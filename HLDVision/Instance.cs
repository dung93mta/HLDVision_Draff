using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision
{

    public static class Instance
    {
        public static class RuntimePolicyHelper
        {
            public static bool LegacyV2RuntimeEnabledSuccessfully { get; private set; }

            static RuntimePolicyHelper()
            {
                ICLRRuntimeInfo clrRuntimeInfo =
                    (ICLRRuntimeInfo)RuntimeEnvironment.GetRuntimeInterfaceAsObject(
                        Guid.Empty,
                        typeof(ICLRRuntimeInfo).GUID);
                try
                {
                    clrRuntimeInfo.BindAsLegacyV2Runtime();
                    LegacyV2RuntimeEnabledSuccessfully = true;
                }
                catch (COMException)
                {
                    // This occurs with an HRESULT meaning 
                    // "A different runtime was already bound to the legacy CLR version 2 activation policy."
                    LegacyV2RuntimeEnabledSuccessfully = false;
                }
            }

            [ComImport]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            [Guid("BD39D1D2-BA2F-486A-89B0-B4B0CB466891")]
            private interface ICLRRuntimeInfo
            {
                void xGetVersionString();
                void xGetRuntimeDirectory();
                void xIsLoaded();
                void xIsLoadable();
                void xLoadErrorString();
                void xLoadLibrary();
                void xGetProcAddress();
                void xGetInterface();
                void xSetDefaultStartupFlags();
                void xGetDefaultStartupFlags();

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void BindAsLegacyV2Runtime();
            }
        }

        public static void Initialize()
        {
            bool isSuccess = RuntimePolicyHelper.LegacyV2RuntimeEnabledSuccessfully;
            AppDomain.CurrentDomain.AssemblyResolve += InitAssembly;
        }

        public static Assembly InitAssembly(object sender, ResolveEventArgs args)
        {
            if (args.Name == null)
                return null;

            if (args.Name.Contains("resources"))
                return null;

            string dllPath = "Dll";
            string dllName = new AssemblyName(args.Name).Name + ".dll";
            string dllFullname = Path.Combine(dllPath, dllName);

            try
            {
                if (File.Exists(dllFullname))
                    return Assembly.LoadFrom(dllFullname);
                else
                {
                    string[] dirs;
                    switch (IntPtr.Size)
                    {
                        case 4:
                            dirs = Directory.GetDirectories(Path.Combine(dllPath, "x86"));
                            break;
                        case 8:
                            dirs = Directory.GetDirectories(Path.Combine(dllPath, "x64"));
                            break;
                        default:
                            throw new Exception("What the hell of this program platform?");
                    }

                    foreach (string dir in dirs)
                    {
                        dllFullname = Path.Combine(dir, dllName);
                        if (File.Exists(dllFullname))
                            return Assembly.LoadFrom(dllFullname);
                    }
                }
            }
            //catch
            //{
            //    return null;
            //}
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Dll Load Error : {0}\r\n\r\n{1}", dllFullname, ex.ToString()));
                return null;
            }
            MessageBox.Show(string.Format("System can not find Dll: {0}\r\nCurrent platform : {1}bit", dllName, (IntPtr.Size * 8).ToString()));
            return null;
        }

    }
}
