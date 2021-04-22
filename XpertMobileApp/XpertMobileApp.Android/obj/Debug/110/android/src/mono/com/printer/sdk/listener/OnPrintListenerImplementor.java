package mono.com.printer.sdk.listener;


public class OnPrintListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.printer.sdk.listener.OnPrintListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_doAfterPrint:()V:GetDoAfterPrintHandler:Com.Printer.Sdk.Listener.IOnPrintListenerInvoker, bindigJarPrinterSPRT\n" +
			"n_doBeforePrint:()V:GetDoBeforePrintHandler:Com.Printer.Sdk.Listener.IOnPrintListenerInvoker, bindigJarPrinterSPRT\n" +
			"n_onReceiveParserData:(I[B)V:GetOnReceiveParserData_IarrayBHandler:Com.Printer.Sdk.Listener.IOnPrintListenerInvoker, bindigJarPrinterSPRT\n" +
			"";
		mono.android.Runtime.register ("Com.Printer.Sdk.Listener.IOnPrintListenerImplementor, bindigJarPrinterSPRT", OnPrintListenerImplementor.class, __md_methods);
	}


	public OnPrintListenerImplementor ()
	{
		super ();
		if (getClass () == OnPrintListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Printer.Sdk.Listener.IOnPrintListenerImplementor, bindigJarPrinterSPRT", "", this, new java.lang.Object[] {  });
	}


	public void doAfterPrint ()
	{
		n_doAfterPrint ();
	}

	private native void n_doAfterPrint ();


	public void doBeforePrint ()
	{
		n_doBeforePrint ();
	}

	private native void n_doBeforePrint ();


	public void onReceiveParserData (int p0, byte[] p1)
	{
		n_onReceiveParserData (p0, p1);
	}

	private native void n_onReceiveParserData (int p0, byte[] p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
