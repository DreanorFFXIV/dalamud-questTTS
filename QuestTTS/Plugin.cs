using System.Speech.Synthesis;
using Dalamud.Game.Command;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin;

namespace QuestTTS
{
    public class Plugin : IDalamudPlugin
    {
        public string Name => "Quest TTS";

        private const string CommandName = "/questtts";

        private DalamudPluginInterface pi;
        private Configuration configuration;
        private PluginUI ui;
        private SpeechSynthesizer synthesizer;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            pi = pluginInterface;

            configuration = pi.GetPluginConfig() as Configuration ?? new Configuration();
            configuration.Initialize(pi);
            synthesizer = configuration.Synthesizer;

            // you might normally want to embed resources and load them from the manifest stream
            ui = new PluginUI(configuration);

            pi.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Opens Settings"
            });

            pi.UiBuilder.OnBuildUi += DrawUI;

            pi.Framework.Gui.Chat.OnChatMessage += HandleChatMessage;
        }

        public void Dispose()
        {
            ui.Dispose();

            pi.CommandManager.RemoveHandler(CommandName);
            pi.Framework.Gui.Chat.OnChatMessage -= HandleChatMessage;
            synthesizer.Dispose();
            pi.Dispose();
        }

        private void OnCommand(string command, string args)
        {
            ui.Visible = true;
        }

        private void DrawUI()
        {
            ui.Draw();
        }

        private void HandleChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if ((int)type == 61 && sender != null && !string.IsNullOrWhiteSpace(sender.TextValue))
            {
                //PluginLog.Log($"senderid:{senderId} | type:{type} | sender:{sender.TextValue} | {message.TextValue}");
                synthesizer.SpeakAsync($"{sender.TextValue} says: {message.TextValue}");
            }
        }
    }
}
