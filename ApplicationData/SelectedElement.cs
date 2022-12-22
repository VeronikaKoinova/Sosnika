using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplicationOptika.ApplicationData
{
    class SelectedElement
    {
        private static string StatusOfUserLabel = "Добро пожаловать, Гость!";
        private static string StatusOfUserButton = "Войти как Гость";


        public static string TakeStatusOfUserLabel(bool status, string NameOfUser)
        {
            if (status)
                StatusOfUserLabel = "Добро пожаловать, " + NameOfUser + "!";
            else
                StatusOfUserLabel = "Добро пожаловать, Гость!";
            return StatusOfUserLabel;
        }

        public static string ReturnStatusOfUserLabel()
        {
            return StatusOfUserLabel;
        }

        public static string TakeStatusOfUserButton(bool status)
        {
            if (status)
                StatusOfUserButton = "Выйти";
            else
                StatusOfUserButton = "Войти как Гость";
            return StatusOfUserButton;
        }

        public static string ReturnStatusOfUserButton()
        {
            return StatusOfUserButton;
        }
    }
}
