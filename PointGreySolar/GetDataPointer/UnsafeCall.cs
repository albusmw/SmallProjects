public class GetPointer
{
    public unsafe static System.IntPtr GetIntPtr(FlyCapture2Managed.ManagedImage ImgSource)
    {
        return (System.IntPtr)ImgSource.data;
    }

}