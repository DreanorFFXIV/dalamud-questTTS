using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Speech.Synthesis;

namespace QuestTTS
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public int Volume { get; set; } = 50;

        public VoiceGender VoiceGender { get; set; } = VoiceGender.Female;
        
        public VoiceAge VoiceAge { get; set; } = VoiceAge.Adult;

        [NonSerialized]
        private DalamudPluginInterface pluginInterface;

        [NonSerialized]
        public SpeechSynthesizer Synthesizer;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
            
            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();
            Synthesizer.SelectVoiceByHints(VoiceGender, VoiceAge);
            Synthesizer.Volume = Volume;
        }

        public void Save()
        {
            pluginInterface.SavePluginConfig(this);
        }
    }
}
