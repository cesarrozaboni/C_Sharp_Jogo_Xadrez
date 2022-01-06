using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jogo_Xadrez.Util
{
    public static class GameUtil
    {
        public static bool ValidaRetorno(Result result)
        {
            if(result.Exception != null)
            {
                Console.WriteLine(result.Exception.Message);
                Console.ReadKey();
                return true;
            }

            return false;
        }

    }
}
