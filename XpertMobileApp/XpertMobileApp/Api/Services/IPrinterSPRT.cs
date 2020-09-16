using System;
using System.Collections.Generic;
using System.Text;


namespace XpertMobileApp.Api.Services
{
    public interface IPrinterSPRT
    {
        bool isConnected();
        bool GetPrinterInstance(EventHandler<EventArgs> ev,string printerName);
        void InitPrinter();
        int PrintText(string content);
        void setPrinter(int comand, int value);
        bool openConnection();
        void closeConnection();
        int sendBytesData(byte[] srcData);
        void setFont(int mCharacterType, int mWidth, int mHeight, int mBold, int mUnderline);
        int getCurrentStatus();
    }
    public static class CommandType
    {
        public static  int INIT_PRINTER = 0;
        public static  int WAKE_PRINTER = 1;
        public static  int PRINT_AND_RETURN_STANDARD = 2;
        public static  int PRINT_AND_NEWLINE = 3;
        public static  int PRINT_AND_ENTER = 4;
        public static  int MOVE_NEXT_TAB_POSITION = 5;
        public static  int DEF_LINE_SPACING = 6;
        public static  int PRINT_AND_WAKE_PAPER_BY_LNCH = 0;
        public static  int PRINT_AND_WAKE_PAPER_BY_LINE = 1;
        public static  int CLOCKWISE_ROTATE_90 = 4;
        public static  int ALIGN = 13;
        public static  int ALIGN_LEFT = 0;
        public static  int ALIGN_CENTER = 1;
        public static  int ALIGN_RIGHT = 2;
        public static  int LINE_HEIGHT = 10;
        public static  int CHARACTER_RIGHT_MARGIN = 11;
        public static  int UNDERLINE = 15;
        public static  int UNDERLINE_OFF = 16;
        public static  int UNDERLINE_ONE_DOTE = 17;
        public static  int UNDERLINE_TWO_DOTE = 18;
        public static  int FONT_MODE = 16;
        public static  int FONT_SIZE = 17;
    }
}
