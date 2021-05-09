using System;
using System.Numerics;
using System.Speech.Synthesis;
using ImGuiNET;

namespace QuestTTS
{
    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    class PluginUI : IDisposable
    {
        private Configuration configuration;

        // this extra bool exists for ImGui, since you can't ref a property
        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        // passing in the image here just for simplicity
        public PluginUI(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void Dispose()
        {
        }

        public void Draw()
        {
            DrawSettingsWindow();
        }

        public void DrawSettingsWindow()
        {
            if (!Visible)
            {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(275, 130), ImGuiCond.Always);
            if (ImGui.Begin("Quest TTS Settings", ref visible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                SetVolume();
                SetGender();
                SetAge();
            }
            ImGui.End();
        }

        private void SetGender()
        {
            string[] items = Enum.GetNames(typeof(VoiceGender));
            int currentItem = Array.IndexOf(items, configuration.VoiceGender.ToString());
            if (ImGui.Combo("Voice Gender", ref currentItem, items, items.Length))
            {
                var gender = (VoiceGender) currentItem + 1;
                configuration.Synthesizer.SelectVoiceByHints(gender);
                configuration.VoiceGender = gender;
                configuration.Save();
            }
        }

        private void SetAge()
        {
            string[] items = Enum.GetNames(typeof(VoiceAge));
            int currentItem = Array.IndexOf(items, configuration.VoiceAge.ToString());
            if (ImGui.Combo("Voice Age", ref currentItem, items, items.Length))
            {
                Enum.TryParse(items[currentItem], out VoiceAge age);
                
                configuration.Synthesizer.SelectVoiceByHints(configuration.VoiceGender, age);
                configuration.VoiceAge = age;
                configuration.Save();
            }
        }
        
        private void SetVolume()
        {
            var volume = configuration.Volume;
            if (ImGui.InputInt("Volume", ref volume))
            {
                configuration.Synthesizer.Volume = volume;
                configuration.Volume = volume;
                configuration.Save();
            }
        }
    }
}
