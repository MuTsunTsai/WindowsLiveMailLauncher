using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace WindowsLiveMailLauncher {
	class Program {

		private const int SW_HIDE=0x00;
		private const int SW_SHOWNORMAL=0x01;
		private const int SW_SHOWMINIMIZED=0x02;
		private const int SW_SHOWMAXIMIZED=0x03;
		private const int SW_SHOW=0x05;
		private const int GWL_EXSTYLE=(-20);
		private const int WS_EX_TOOLWINDOW=0x0080;
		private const int WS_EX_APPWINDOW=0x00040000;

		[DllImport("User32.dll")]
		private static extern int ShowWindow(IntPtr hwnd, int command);

		[DllImport("User32.dll", EntryPoint="FindWindow")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", SetLastError=true)]
		static extern int GetWindowLong(IntPtr hWnd, int nIndex);
	
		static void Main(string[] args) {
			// 執行 Windows Live Mail
			ProcessStartInfo start=new ProcessStartInfo();
			start.FileName="C:\\Program Files\\Windows Live\\Mail\\wlmail.exe";
			start.WindowStyle=ProcessWindowStyle.Minimized;
			start.CreateNoWindow=true;
			Process.Start(start);
			
			// 一偵測到主視窗物件被載入，就搶先將它設定成工具視窗，使得它不會因為載入而出現在工作列上
			IntPtr hWnd;
			do hWnd=FindWindow("Outlook Express Browser Class", null);
			while(hWnd.Equals(IntPtr.Zero));
			int windowStyle=GetWindowLong(hWnd, GWL_EXSTYLE);
			SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_TOOLWINDOW);
			ShowWindow(hWnd, SW_HIDE);
			
			// 隔一秒之後再把該視窗的樣式還原成員本的樣子
			Thread.Sleep(1000);
			SetWindowLong(hWnd, GWL_EXSTYLE, windowStyle);
			ShowWindow(hWnd, SW_HIDE);
		}
	}
}
