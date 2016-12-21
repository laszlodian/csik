using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;


namespace e77.MeasureBase.GUI
{
    public class MouseClickHacker
    {
        private MouseClickHacker()
        {
        }

        static string HackFileName;

        static Object[] Keys = new Object[0];
        static string[] Values = new string[0];

        static MouseClickHacker()
        {
            string path =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                Path.DirectorySeparatorChar +
                Assembly.GetEntryAssembly().GetName().Name +
                Path.DirectorySeparatorChar;

            Directory.CreateDirectory(path);
            HackFileName = path + "MouseClickHack.txt";

            if (File.Exists(HackFileName))
            {
                Values = File.ReadAllLines(HackFileName);
            }
        }

        private static int IndexOf(Object key)
        {
            if (Values.Length < Keys.Length)
            {
                Array.Resize(ref Values, Keys.Length);
            }

            for (int i = 0; i < Keys.Length; i++)
            {
                if (Keys[i] is string)
                {
                    if (((string)Keys[i]).Equals(((Control)key).Name))
                    {
                        return i;
                    }
                }
                else
                    if (Keys[i] is Control)
                {
                    if (Keys[i] == key)
                    {
                        return i;
                    }
                }
                else
                {
                    throw new Exception("Gebasz van. A kulcs csak string vagy control lehet");
                }
            }
            return -1;
        }

        private static void Control_MouseClick(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;

            int i = IndexOf(control);

            if (i < 0) throw new Exception("Gebasz van, ilyen hiba nem is lehetne. Nincs a keresett Control a listában!");

            if (Control.ModifierKeys == (System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift))
            {
                String value = WebUtility.HtmlDecode(Values[IndexOf(control)]);

                if (InputQuery.Show(control.Name, "Érték:", ref value))
                {
                    Values[i] = WebUtility.HtmlEncode(value);
                    File.WriteAllLines(HackFileName, Values);
                }
            }
            else if (Control.ModifierKeys == (System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Control))
            {
                control.Text = WebUtility.HtmlDecode(Values[i]);
            }
        }

        /// <summary>
        /// Trükkös egéreseményt tud hozzáadni egy vezérlőhöz.
        /// Debug esetén használható, tipikusan gyáriszám beviteli mezőknél, hogy ne kelljen állandóan ugyanazt a gyáriszámot beírni.
        /// Ctrl+Alt+Klikk: Az előzőleg eltárolt szöveget teszi az adott vezérlőbe
        /// Shift+Ctrl+Alt+Klikk: Új szöveget lehet eltárolni, amit egy felugró ablakban lehet megadni.
        /// A szövegeket a felhasználó application data roaming mappájába, azon belül általában az exe nevével megegyező mappába teszi.
        /// SharedByName  
        /// true: ha sok kis egyforma gyermek ablakban az ugyanolyan nevű vezérlőket közös szöveggel szeretnénk használni. Pl.: UA3 járató SN
        /// false: minden vezérlőhöz külön szöveg tartozik, akkor is, ha a neveik egyformák.
        /// egy-egy vezérlőnél persze édestökmindegy (Pl. TerminalTester)
        /// </summary>
        /// <param name="control">A vezérlő, amihez az egér eseményt kell rendelni</param>
        /// <param name="SharedByName">   </param>
        public static void Add(Control control, bool SharedByName)
        {
            control.MouseClick += Control_MouseClick;

            int i = IndexOf(control);
            if (i >= 0)
            {
                if (SharedByName)
                    return;
                throw new Exception("Ezt a controlt már hozzáadtad a listához!");
            }
            i = Keys.Length;
            Array.Resize(ref Keys, i + 1);

            if (SharedByName)
            {
                if (string.IsNullOrEmpty(control.Name))
                    throw new Exception("SharedByName=true esetén a controlnak kötelező nevet adni!");
                Keys[i] = control.Name;
            }
            else
            {
                Keys[i] = control;
            }
        }

        /* 
         public static void Add(System.Windows.Controls control, bool SharedByName)
         {

         }          */
    }
}
