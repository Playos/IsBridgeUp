using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;

namespace rlel
{
    class Primary
    {

        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        public static void Main(string[] args)
        {
            rlel.App app = new rlel.App();
            if (args.Length == 0)
            {
                app.Run(new MainWindow());
            }
            else
            {
                //MessageBox.Show(string.Join(" ", args));
                try
                {
                    MainWindow PlaceHolder = new MainWindow();
                    string ArgUser = args[0];
                    RijndaelManaged rjm = new RijndaelManaged();

                    foreach (string credentials in Properties.Settings.Default.accounts)
                    {
                        Account account = new Account(PlaceHolder);
                        string[] split = credentials.Split(new char[] { ':' }, 4);
                        account.username.Text = split[0];

                        if (account.username.Text == ArgUser)
                        {
                            string key = PlaceHolder.getKey();
                            string iv = PlaceHolder.getIV();
                            rjm.Key = Convert.FromBase64String(key);
                            rjm.IV = Convert.FromBase64String(iv);

                            account.password.Password = PlaceHolder.decryptPass(rjm, split[1]);
                            account.launchAccount(false, Path.Combine(PlaceHolder.getTranqPath(), "bin", "Exefile.exe"), false, ArgUser, account.password.SecurePassword);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }



            }

        }
    }
}
