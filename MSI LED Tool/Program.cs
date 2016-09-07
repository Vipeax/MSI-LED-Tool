using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MSI_LED_Tool
{
    class Program
    {
        private const string SettingsFileName = "Settings.json";

        [DllImport("Lib\\NDA.dll", CharSet = CharSet.Unicode)]
        private static extern bool NDA_GetGPUCounts(out long gpuCounts);

        [DllImport("Lib\\NDA.dll", CharSet = CharSet.Unicode)]
        private static extern bool NDA_GetGraphicsInfo(int iAdapterIndex, out NdaGraphicsInfo graphicsInfo);

        [DllImport("Lib\\NDA.dll", CharSet = CharSet.Unicode)]
        private static extern bool NDA_SetIlluminationParmColor_RGB(int iAdapterIndex, int cmd, int led1, int led2, int ontime, int offtime, int time, int darktime, int bright, int r, int g, int b, bool one = false);

        private static Thread updateThreadFront;
        private static Thread updateThreadBack;
        private static Thread updateThreadSide;

        private static List<int> adapterIndexes;

        private static bool vgaMutex;
        private static Color ledColor;
        private static AnimationType animationType;

        static void Main(string[] args)
        {
            string settingsFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\{SettingsFileName}";

            if (File.Exists(settingsFile))
            {
                using (var sr = new StreamReader(settingsFile))
                {
                    var settings = JsonSerializer<LedSettings>.DeSerialize(sr.ReadToEnd());

                    if (settings != null)
                    {
                        ledColor = Color.FromArgb(255, settings.R, settings.G, settings.B);
                        animationType = settings.AnimationType;
                    }
                }
            }
            else
            {
                using (var sw = new StreamWriter(settingsFile, false))
                {
                    sw.WriteLine(JsonSerializer<LedSettings>.Serialize(new LedSettings { R = 255, G = 0, B = 0, AnimationType = AnimationType.NoAnimation }));
                }
            }

            if (ledColor == null)
            {
                ledColor = Color.Red;
                animationType = AnimationType.NoAnimation;
            }

            adapterIndexes = new List<int>();
            long gpuCount;
            bool canGetGpuCount = NDA_GetGPUCounts(out gpuCount);
            if (canGetGpuCount == false)
            {
                return;
            }

            for (int i = 0; i < gpuCount; i++)
            {
                NdaGraphicsInfo graphicsInfo;
                if (NDA_GetGraphicsInfo(i, out graphicsInfo) == false)
                {
                    return;
                }

                string vendorCode = graphicsInfo.Card_pDeviceId.Substring(4, 4).ToUpper();
                string deviceCode = graphicsInfo.Card_pDeviceId.Substring(0, 4).ToUpper();
                string subVendorCode = graphicsInfo.Card_pSubSystemId.Substring(4, 4).ToUpper();
                //nVidia
                if (vendorCode.Equals("10DE", StringComparison.OrdinalIgnoreCase)
                    //1080                                                           //1070
                    && (deviceCode.Equals("1B80", StringComparison.OrdinalIgnoreCase) || deviceCode.Equals("1B81", StringComparison.OrdinalIgnoreCase))
                    //MSI
                    && subVendorCode.Equals("1462", StringComparison.OrdinalIgnoreCase))
                {
                    adapterIndexes.Add(i);
                }
            }

            if (adapterIndexes.Count > 0)
            {
                updateThreadFront = new Thread(UpdateLedsFront);
                updateThreadSide = new Thread(UpdateLedsSide);
                updateThreadBack = new Thread(UpdateLedsBack);
                updateThreadFront.Start();
                updateThreadSide.Start();
                updateThreadBack.Start();
            }

            while (true)
            {
                Thread.CurrentThread.Join(new TimeSpan(1, 0, 0));
            }
        }

        private static void UpdateLedsFront()
        {
            while (true)
            {
                switch (animationType)
                {
                    case AnimationType.NoAnimation:
                        UpdateLeds(21, 4, 4);
                        break;
                    case AnimationType.Breathing:
                        UpdateLeds(27, 4, 7);
                        break;
                    case AnimationType.Flashing:
                        UpdateLeds(28, 4, 0, 25, 100);
                        break;
                    case AnimationType.DoubleFlashing:
                        UpdateLeds(30, 4, 0, 10, 10, 91);
                        break;
                }
            }
        }


        private static void UpdateLedsSide()
        {
            while (true)
            {
                switch (animationType)
                {
                    case AnimationType.NoAnimation:
                        UpdateLeds(21, 1, 4);
                        break;
                    case AnimationType.Breathing:
                        UpdateLeds(27, 1, 7);
                        break;
                    case AnimationType.Flashing:
                        UpdateLeds(28, 1, 0, 25, 100);
                        break;
                    case AnimationType.DoubleFlashing:
                        UpdateLeds(30, 1, 0, 10, 10, 91);
                        break;
                }
            }
        }

        private static void UpdateLedsBack()
        {
            while (true)
            {
                switch (animationType)
                {
                    case AnimationType.NoAnimation:
                        UpdateLeds(21, 2, 4);
                        break;
                    case AnimationType.Breathing:
                        UpdateLeds(27, 2, 7);
                        break;
                    case AnimationType.Flashing:
                        UpdateLeds(28, 2, 0, 25, 100);
                        break;
                    case AnimationType.DoubleFlashing:
                        UpdateLeds(30, 2, 0, 10, 10, 91);
                        break;
                }
            }
        }

        private static void UpdateLeds(int cmd, int ledId, int time, int ontime = 0, int offtime = 0, int darkTime = 0)
        {
            for (int i = 0; i < adapterIndexes.Count; i++)
            {
                Thread.CurrentThread.Join(10);
                for (int index = 0; vgaMutex && index < 100; ++index)
                {
                    Thread.CurrentThread.Join(5);
                }
                vgaMutex = true;
                Thread.CurrentThread.Join(20);

                bool oneCall = animationType != AnimationType.NoAnimation;

                NDA_SetIlluminationParmColor_RGB(i, cmd, ledId, 0, ontime, offtime, time, darkTime, 0, ledColor.R, ledColor.G, ledColor.B, oneCall);

                vgaMutex = false;
            }

            Thread.CurrentThread.Join(2000);
        }
    }
}