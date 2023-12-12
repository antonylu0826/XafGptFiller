using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace XafGptFiller.Module.Controllers
{
    public class EmployeeDetailViewController : ViewController<DetailView>
    {
        readonly PopupWindowShowAction FillDataAction;

        public EmployeeDetailViewController()
        {
            FillDataAction = new PopupWindowShowAction()
            {
                Id = "FillDataAction",
                Caption = "Fill Data by AI",
            };
            FillDataAction.CustomizePopupWindowParams += FillDataAction_CustomizePopupWindowParams;
            FillDataAction.Execute += FillDataAction_Execute;
            Actions.Add(FillDataAction);
        }

        private void FillDataAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(DataContent));
            var content = os.CreateObject<DataContent>();
            DetailView detailView = Application.CreateDetailView(os, content);
            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.View = detailView;
        }

        private async void FillDataAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var dc = e.PopupWindowViewCurrentObject as DataContent;

            if (!string.IsNullOrEmpty(dc.Content))
            {
                var properties = string.Join(", ",
                View.ObjectTypeInfo.Members
                    .Where(m => m.IsPublic && m.IsPersistent && m.IsProperty && !m.IsReadOnly)
                    .Select(s => s.BindingName).ToList());

                var result = await OpenAiDataFiller(properties, dc.Content);
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                var oType = View.CurrentObject.GetType();
                

                foreach ( var kvp in values )
                {
                    if (View.ObjectTypeInfo.Members.Where(m => m.BindingName == kvp.Key).Any())
                    {
                        var pinfo = oType.GetProperty(kvp.Key);
                        pinfo?.SetValue(View.CurrentObject, Convert.ChangeType(kvp.Value, pinfo.PropertyType));
                    }
                    
                }
            }
        }

        static async Task<string> OpenAiDataFiller(string properties, string content)
        {
            var openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = "YOUR OPEN_AI KEY"
            });

            var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem($@"You are a data auto-filler. I will provide you with a set of data structures, and then provide you with a piece of spoken text. I will also ask you to break down the content and find the appropriate fields to fill in according to the data structure, and provide it to I got the result, the format of the result is JSON.
                Precautions:
                1. Please keep the name of the data structure field and do not change it. I need it as the corresponding condition.
                2. If the result content is null, there is no need to return it.
                3. Do not add data structure fields that do not exist
                4. Just reply me with the result, no other explanation in result is needed, which is convenient for my program to use.
                5. There should be no annotations in the result content.
                6. The date field should be expressed in date format.

                Data structure:
                {properties}
                "),

                    ChatMessage.FromUser(content)
                },
                Model = Models.Gpt_3_5_Turbo,
                MaxTokens = 100//optional
            });
            if (completionResult.Successful)
            {
                return completionResult.Choices.First().Message.Content;
            }
            return null;
        }

    }
}
