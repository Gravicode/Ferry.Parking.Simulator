using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DTK.LPR.Lib
{
    public class DTKLPR4
    {
        const string dllName = "DTKLPR4.dll";
        static DTKLPR4()
        {
            var is64 = IntPtr.Size == 8;
            string assembly_path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(DTKLPR4)).Location);
            string library_path = is64 ? assembly_path + "\\x64\\" : assembly_path + "\\x86\\";

            SetDllDirectory(library_path);
            IntPtr res = LoadLibrary(dllName);
            SetDllDirectory("");
            if (res == IntPtr.Zero)
            {
                LoadLibrary(dllName);
            }
        }

        #region DLL Imports

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        // =====================
        // Callbacks
        // =====================

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void LicensePlateDetectedNative(IntPtr engine, IntPtr plate);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FrameCapturedNative(IntPtr videoCap, IntPtr pBuffer, int width, int height, int stride, PIXFMT pixelFormat, long timestamp, IntPtr customObject);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CaptureErrorNative(IntPtr videoCap, ERR_CAPTURE errorCode, IntPtr customObject);
      
        // =====================
        // Video Capture
        // =====================

        [DllImport(dllName)]
        public static extern IntPtr VideoCapture_Create(IntPtr frameCapturedCallback, IntPtr captureErrorCallback, IntPtr customObject);

        [DllImport(dllName)]
        public static extern void VideoCapture_Destroy(IntPtr hVideoCapture);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern int VideoCapture_StartCaptureFromFile(IntPtr hVideoCapture, string fileName, int repeat_count);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern int VideoCapture_StartCaptureFromIPCamera(IntPtr hVideoCapture, string ipCameraURL);

        [DllImport(dllName)]
        public static extern int VideoCapture_StartCaptureFromDevice(IntPtr hVideoCapture, int deviceIndex, int captureWidth, int captureHeight);

        [DllImport(dllName)]
        public static extern int VideoCapture_StopCapture(IntPtr hVideoCapture);

        // =====================
        // LPREngine
        // =====================
        [DllImport(dllName)]
        public static extern IntPtr LPREngine_Create(IntPtr hParams, bool bVideo, IntPtr plateDetectedCallback);

        [DllImport(dllName)]
        public static extern void LPREngine_Destroy(IntPtr hEngine);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern IntPtr LPREngine_ReadFromFile(IntPtr hEngine, string fileName);

        [DllImport(dllName)]
        public static extern IntPtr LPREngine_ReadFromImageBuffer(IntPtr hEngine, IntPtr pBuffer, int width, int height, int stride, PIXFMT pixelFormat);

        [DllImport(dllName)]
        public static extern int LPREngine_PutFrameImageBuffer(IntPtr hEngine, IntPtr pBuffer, int width, int height, int stride, PIXFMT pixelFormat, long timestamp, long customData);

        [DllImport(dllName)]
        public static extern int LPREngine_GetProcessingFPS(IntPtr hEngine);

        [DllImport(dllName)]
        public static extern int LPREngine_IsLicensed(IntPtr hEngine);

        // ==================
        // LPRParams
        // ==================
        [DllImport(dllName)]
        public static extern IntPtr LPRParams_Create();

        [DllImport(dllName)]
        public static extern void LPRParams_Destroy(IntPtr hParams);

        [DllImport(dllName)]
        public static extern int LPRParams_get_MinCharHeight(IntPtr hParams);

        [DllImport(dllName)]
        public static extern void LPRParams_set_MinCharHeight(IntPtr hParams, int val);

        [DllImport(dllName)]
        public static extern int LPRParams_get_MaxCharHeight(IntPtr hParams);

        [DllImport(dllName)]
        public static extern void LPRParams_set_MaxCharHeight(IntPtr hParams, int val);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern int LPRParams_get_Countries(IntPtr hParams, StringBuilder val, int val_len);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern void LPRParams_set_Countries(IntPtr hParams, string val);

        [DllImport(dllName)]
        public static extern int LPRParams_get_NumThreads(IntPtr hParams);

        [DllImport(dllName)]
        public static extern void LPRParams_set_NumThreads(IntPtr hParams, int val);

        [DllImport(dllName)]
        public static extern int LPRParams_get_FPSLimit(IntPtr hParams);

        [DllImport(dllName)]
        public static extern void LPRParams_set_FPSLimit(IntPtr hParams, int val);

        [DllImport(dllName)]
        public static extern int LPRParams_get_DuplicateResultsDelay(IntPtr hParams);

        [DllImport(dllName)]
        public static extern void LPRParams_set_DuplicateResultsDelay(IntPtr hParams, int val);

        [DllImport(dllName)]
        public static extern int LPRParams_get_ResultConfirmationsCount(IntPtr hParams);

        [DllImport(dllName)]
        public static extern void LPRParams_set_ResultConfirmationsCount(IntPtr hParams, int val);

        [DllImport(dllName)]
        public static extern bool LPRParams_get_RecognitionOnMotion(IntPtr hParams);

        [DllImport(dllName)]
        public static extern void LPRParams_set_RecognitionOnMotion(IntPtr hParams, bool val);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern int LPRParams_get_BurnFormatString(IntPtr hParmas, StringBuilder val, int val_len);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern void LPRParams_set_BurnFormatString(IntPtr hParmas, string val);

        [DllImport(dllName)]
        public static extern int LPRParams_get_BurnPosition(IntPtr hParams);

        [DllImport(dllName)]
        public static extern void LPRParams_set_BurnPosition(IntPtr hParams, int val);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern void LPRParams_GetXOption(IntPtr hParmas, string optionName, StringBuilder val, int val_len);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern void LPRParams_SetXOption(IntPtr hParmas, string optionName, string val);

        [DllImport(dllName)]
        public static extern int LPRParams_GetZonesCount(IntPtr hParams);

        [DllImport(dllName)]
        public static extern int LPRParams_AddZone(IntPtr hParams);

        [DllImport(dllName)]
        public static extern int LPRParams_RemoveZone(IntPtr hParams, int zone);

        [DllImport(dllName)]
        public static extern int LPRParams_GetZonePointsCount(IntPtr hParams, int zone);

        [DllImport(dllName)]
        public static extern void LPRParams_GetZonePoint(IntPtr hParams, int zone, int pointNum, out int x, out int y);
  
        [DllImport(dllName)]
        public static extern int LPRParams_AddZonePoint(IntPtr hParams, int zone, int x, int y);

        [DllImport(dllName)]
        public static extern int LPRParams_RemoveZonePoint(IntPtr hParams, int zone, int pointNum);

        // ==================
        // LPRResult
        // ==================

        [DllImport(dllName)]
        public static extern void LPRResult_Destroy(IntPtr hResult);

        [DllImport(dllName)]
        public static extern int LPRResult_GetPlatesCount(IntPtr hResult);

        [DllImport(dllName)]
        public static extern IntPtr LPRResult_GetPlate(IntPtr hResult, int index);


        // ==================
        // LicensePlate
        // ==================

        [DllImport(dllName)]
        public static extern int LicensePlate_Destroy(IntPtr hPlate);

        [DllImport(dllName, CharSet = CharSet.Auto)]
        public static extern void LicensePlate_GetText(IntPtr hPlate, IntPtr text, int maxLen);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern void LicensePlate_GetCountryCode(IntPtr hPlate, StringBuilder code, int maxLen);

        [DllImport(dllName)]
        public static extern int LicensePlate_GetX(IntPtr hPlate);

        [DllImport(dllName)]
        public static extern int LicensePlate_GetY(IntPtr hPlate);

        [DllImport(dllName)]
        public static extern int LicensePlate_GetWidth(IntPtr hPlate);

        [DllImport(dllName)]
        public static extern int LicensePlate_GetHeight(IntPtr hPlate);

        [DllImport(dllName)]
        public static extern int LicensePlate_GetConfidence(IntPtr hPlate);

        [DllImport(dllName)]
        public static extern int LicensePlate_GetZone(IntPtr hPlate);

        [DllImport(dllName)]
        public static extern int LicensePlate_GetZoneName(IntPtr hPlate, StringBuilder zoneName, int maxLen);

        [DllImport(dllName)]
        public static extern int LicensePlate_GetDirection(IntPtr hPlate);

        [DllImport(dllName)]
        public static extern long LicensePlate_GetTimestamp(IntPtr hPlate);

        [DllImport(dllName)]
        public static extern long LicensePlate_GetId(IntPtr hPlate);

        [DllImport(dllName)]
        public static extern long LicensePlate_GetCustomData(IntPtr hPlate);

        [DllImport(dllName)]
        public static extern void LicensePlate_GetImageBuffer(IntPtr hPlate, out IntPtr pBuffer, out int width, out int height, out int stride);

        [DllImport(dllName)]
        public static extern void LicensePlate_GetPlateImageBuffer(IntPtr hPlate, out IntPtr pBuffer, out int width, out int height, out int stride);

        [DllImport(dllName)]
        public static extern void LicensePlate_FreeImageBuffer(IntPtr pBuffer);
      
        // ==================
        // License
        // ==================
        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern int ActivateLicenseOnline(string licenseKey, string comments);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern void GetActivatedLicenseInfo(StringBuilder licenseKey, int licenseKeyMaxLen, StringBuilder comments, int commentsMaxLen, out int channels, out ulong expirationDate);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern int GetSystemID(StringBuilder systemID, int systemIDMaxLen);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern int ActivateLicenseOffline(string activationCode);

        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern int GetActivationLink(string licenseKey, string comments, StringBuilder activationLink, int activationLinkMaxLen);


        // =========================
        // Get Supported countries
        // =========================
        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern int GetSupportedCountries(StringBuilder buffer, int bufferSize);

        // =========================
        // Get library version
        // =========================
        [DllImport(dllName, CharSet = CharSet.Ansi)]
        public static extern int GetLibraryVersion(StringBuilder buffer, int bufferSize);

        #endregion

        public static List<string> GetSupportedCountries()
        {
            int required_size = DTKLPR4.GetSupportedCountries(null, 0);
            StringBuilder builder = new StringBuilder(required_size);
            DTKLPR4.GetSupportedCountries(builder, required_size);
            List<string> res = new List<string>();
            foreach (string item in builder.ToString().Split(';'))
            {
                res.Add(item);
            }
            return res;
        }

        public static string GetLibraryVersion()
        {
            int required_size = DTKLPR4.GetLibraryVersion(null, 0);
            StringBuilder builder = new StringBuilder(required_size);
            DTKLPR4.GetLibraryVersion(builder, required_size);
            return builder.ToString();
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, int count);

        public static Bitmap CreateBitmapFromBuffer(IntPtr pBuffer, int width, int height, int stride, PIXFMT pixelFormat)
        {
            // Create bitmap and copy pixels data
            PixelFormat format;
            if (pixelFormat == PIXFMT.BGR24)
                format = PixelFormat.Format24bppRgb;
            else if (pixelFormat == PIXFMT.RGB24)
                format = PixelFormat.Format24bppRgb;
            else if (pixelFormat == PIXFMT.GRAYSCALE)
                format = PixelFormat.Format8bppIndexed;
            else
                return null;
            Bitmap bmp = new Bitmap(width, height, format);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            IntPtr dst = bmpData.Scan0;
            IntPtr src = pBuffer;
            for (int i = 0; i < height; i++)
            {
                CopyMemory(dst, src, stride);
                dst = new IntPtr(dst.ToInt64() + bmpData.Stride);
                src = new IntPtr(src.ToInt64() + stride);
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }

    public delegate void LicensePlateDetected(LPREngine engine, LicensePlate plate);

    public delegate void FrameCaptured(VideoCapture videoCap, IntPtr pBuffer, int width, int height, int stride, PIXFMT pixelFormat, long timestamp, object customObject);

    public delegate void CaptureError(VideoCapture videoCap, ERR_CAPTURE errorCode, object customObject);      

     // image buffer pixel formats
    public enum PIXFMT
    {
        GRAYSCALE = 1,
        RGB24 = 2,
        BGR24 = 3,
        YUV420 = 4,
    }

    // format string burn position
    public enum BURN_POS
    {
        LEFT_TOP = 0,
        RIGHT_TOP = 1,
        LEFT_BOTTOM = 2,
        RIGHT_BOTTOM = 3,
    }

    // video capture error codes
    public enum ERR_CAPTURE
    {
        OPEN_VIDEO = 1,
        READ_FRAME = 2,
        EOF = 3,
    }

    /// <summary>
    /// License class
    /// </summary>
   
    public class License
    {
        /// <summary>
        /// Activates license key online
        /// </summary>
        /// <param name="licenseKey">license key</param>
        /// <param name="comments">comments string (optional)</param>
        /// <returns>returns 0 on success, an error code otherwise</returns>
        static public int ActivateLicenseOnline(string licenseKey, string comments)
        {
            return DTKLPR4.ActivateLicenseOnline(licenseKey, comments);
        }

        /// <summary>
        /// Gets activated license information
        /// </summary>
        /// <param name="licenseKey">license key</param>
        /// <param name="comments">comments</param>
        /// <param name="channels">maximum number of channels</param>
        /// <param name="expirationDate">expiration date (for time limited licenses)</param>
        /// <returns>returns 0 on success, an error code otherwise</returns>     
        static public void GetActivatedLicenseInfo(out string licenseKey, out string comments, out int channels, out DateTime expirationDate)
        {
            StringBuilder licKey = new StringBuilder(50);
            StringBuilder comm = new StringBuilder(255);
            ulong expDate_time_t;

            DTKLPR4.GetActivatedLicenseInfo(licKey, 50, comm, 255, out channels, out expDate_time_t);

            if (expDate_time_t != 0)
            {
                expirationDate = new DateTime(1970, 1, 1).ToLocalTime().AddSeconds(expDate_time_t);
                if (expirationDate.Year < 2000)
                    expirationDate = DateTime.MaxValue;
            }
            else
            {
                expirationDate = DateTime.MaxValue;
            }
            licenseKey = licKey.ToString();
            comments = comm.ToString();
        }

        /// <summary>
        /// Returns system ID (hardware ID)
        /// </summary>
        /// <returns>system ID string</returns>
        static public string GetSystemID()
        {
            StringBuilder sysID = new StringBuilder(50);
            DTKLPR4.GetSystemID(sysID, 50);
            return sysID.ToString();
        }

        /// <summary>
        /// Activates license using activation code
        /// </summary>
        /// <param name="activationCode">activation code</param>
        /// <returns>returns 0 on success, an error code otherwise</returns>
        static public int ActivateLicenseOffline(string activationCode)
        {
            return DTKLPR4.ActivateLicenseOffline(activationCode);
        }

        static public string GetActivationLink(string licenseKey, string comments)
        {
            StringBuilder actLink = new StringBuilder(255);
            if (DTKLPR4.GetActivationLink(licenseKey, comments, actLink, 255) == 0)
            {
                return actLink.ToString();
            }
            return "";
        }
    }

    /// <summary>
    /// LPR parameters
    /// </summary>
    public class LPRParams
    {
        internal IntPtr hParmas = IntPtr.Zero;
        /// <summary>
        /// Creates new instance of LPRParams.
        /// </summary>
        public LPRParams()
        {
            this.hParmas = DTKLPR4.LPRParams_Create();
        }

        ~LPRParams()
        {
            DTKLPR4.LPRParams_Destroy(this.hParmas);
        }

        /// <summary>
        /// Minimum character height on plate in pixels.
        /// </summary>
        public int MinCharHeight
        {
            get
            {
                return DTKLPR4.LPRParams_get_MinCharHeight(this.hParmas);
            }
            set
            {
                DTKLPR4.LPRParams_set_MinCharHeight(this.hParmas, value);
            }
        }

        /// <summary>
        /// Maximum character height on plate in pixels.
        /// </summary>
        public int MaxCharHeight
        {
            get
            {
                return DTKLPR4.LPRParams_get_MaxCharHeight(this.hParmas);
            }
            set
            {
                DTKLPR4.LPRParams_set_MaxCharHeight(this.hParmas, value);
            }
        }

        /// <summary>
        /// Frames per second (FPS) limit. Suitable for video mode only.
        /// </summary>
        public int FPSLimit
        {
            get
            {
                return DTKLPR4.LPRParams_get_FPSLimit(this.hParmas);
            }
            set
            {
                DTKLPR4.LPRParams_set_FPSLimit(this.hParmas, value);
            }
        }

        /// <summary>
        /// Defines minimum delay in milliseconds to allow duplicate results. Suitable for video mode only.
        /// </summary>
        public int DuplicateResultsDelay
        {
            get
            {
                return DTKLPR4.LPRParams_get_DuplicateResultsDelay(this.hParmas);
            }
            set
            {
                DTKLPR4.LPRParams_set_DuplicateResultsDelay(this.hParmas, value);
            }
        }

        /// <summary>
        /// Enables recognition on motion feature. Suitable for video mode only.
        /// </summary>
        public bool RecognitionOnMotion
        {
            get
            {
                return DTKLPR4.LPRParams_get_RecognitionOnMotion(this.hParmas);
            }
            set
            {
                DTKLPR4.LPRParams_set_RecognitionOnMotion(this.hParmas, value);
            }
        }

        /// <summary>
        /// Defines number of conformations of the same plate number to return the result. Suitable for video mode only.
        /// </summary>
        public int ResultConfirmationsCount
        {
            get
            {
                return DTKLPR4.LPRParams_get_ResultConfirmationsCount(this.hParmas);
            }
            set
            {
                DTKLPR4.LPRParams_set_ResultConfirmationsCount(this.hParmas, value);
            }
        }

        /// <summary>
        /// Defines number of threads for video processing. Suitable for video mode only.
        /// </summary>
        public int NumThreads
        {
            get
            {
                return DTKLPR4.LPRParams_get_NumThreads(this.hParmas);
            }
            set
            {
                DTKLPR4.LPRParams_set_NumThreads(this.hParmas, value);
            }
        }


        public string BurnFormatString
        {
            get
            {        
                int size = DTKLPR4.LPRParams_get_BurnFormatString(this.hParmas, null, 0);
                if (size > 0)
                {
                    StringBuilder builder = new StringBuilder(size + 1);
                    DTKLPR4.LPRParams_get_BurnFormatString(this.hParmas, builder, builder.Capacity);
                    return builder.ToString();
                }
                return "";
            }
            set
            {
                DTKLPR4.LPRParams_set_BurnFormatString(this.hParmas, value);
            }
        }

        public int BurnPosition
        {
            get
            {
                return DTKLPR4.LPRParams_get_BurnPosition(this.hParmas);
            }
            set
            {
                DTKLPR4.LPRParams_set_BurnPosition(this.hParmas, value);
            }
        }

        /// <summary>
        /// Gets number of recognition zones.
        /// </summary>
        /// <returns>number of recognition zones</returns>
        public int GetZonesCount()
        {
            return DTKLPR4.LPRParams_GetZonesCount(this.hParmas);
        }

        /// <summary>
        /// Adds new recognition zone.
        /// </summary>
        /// <returns>index of added zone</returns>
        public int AddZone()
        {
            return DTKLPR4.LPRParams_AddZone(this.hParmas);
        }

        /// <summary>
        /// Removes recognition zone by index
        /// </summary>
        /// <param name="zoneIndex">zone index</param>
        public void RemoveZone(int zoneIndex)
        {
            DTKLPR4.LPRParams_RemoveZone(this.hParmas, zoneIndex);
        }

        /// <summary>
        /// Gets number of points for specific recogntion zone.
        /// </summary>
        /// <param name="zoneIndex">index of zone</param>
        /// <returns>number of points</returns>
        public int GetZonePointsCount(int zoneIndex)
        {
            return DTKLPR4.LPRParams_GetZonePointsCount(this.hParmas, zoneIndex);
        }

        /// <summary>
        /// Gets point coordinates for specific recognition zone.
        /// </summary>
        /// <param name="zoneIndex">zone index</param>
        /// <param name="pointIndex">point index</param>
        /// <param name="x">x-coordinate (out parameter)</param>
        /// <param name="y">y-coordinate (out parameter)</param>
        public void GetZonePoint(int zoneIndex, int pointIndex, out int x, out int y)
        {
            DTKLPR4.LPRParams_GetZonePoint(this.hParmas, zoneIndex, pointIndex, out x, out y);
        }

        /// <summary>
        /// Adds new point for specific recognition zone.
        /// </summary>
        /// <param name="zoneIndex">zone index</param>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <returns></returns>
        public int AddZonePoint(int zoneIndex, int x, int y)
        {
            return DTKLPR4.LPRParams_AddZonePoint(this.hParmas, zoneIndex, x, y);
        }

        /// <summary>
        /// Removes point for specific recognition zone.
        /// </summary>
        /// <param name="zoneIndex">zone inxed</param>
        /// <param name="pointIndex">point index</param>
        /// <returns></returns>
        public int RemoveZonePoint(int zoneIndex, int pointIndex)
        {
            return DTKLPR4.LPRParams_RemoveZonePoint(this.hParmas, zoneIndex, pointIndex);
        }

        /// <summary>
        /// The list of countries (comma delimited string) defined for recognition.
        /// </summary>
        public string Countries
        {
            get
            {        
                int size = DTKLPR4.LPRParams_get_Countries(this.hParmas, null, 0);
                if (size > 0)
                {
                    StringBuilder builder = new StringBuilder(size + 1);
                    DTKLPR4.LPRParams_get_Countries(this.hParmas, builder, builder.Capacity);
                    return builder.ToString();
                }
                return "";
            }
            set
            {
                DTKLPR4.LPRParams_set_Countries(this.hParmas, value);
            }
        }


        public void SetXOption(string optionName, string val)
        {
            DTKLPR4.LPRParams_SetXOption(this.hParmas, optionName, val);
        }

        public string GetXOption(string optionName)
        {
            StringBuilder builder = new StringBuilder(255);
            DTKLPR4.LPRParams_GetXOption(this.hParmas, optionName, builder, 255);
            return builder.ToString();
        }

    }

    /// <summary>
    /// LicensePlate class
    /// </summary>
    public class LicensePlate : IDisposable
    {
        internal IntPtr hPlate = IntPtr.Zero;

        internal LicensePlate(IntPtr hPlate)
        {
            this.hPlate = hPlate;
        }

        /// <summary>
        /// Destructor of LPREngine instance
        /// </summary>
        ~LicensePlate()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        /// <summary>
        /// Releases resources of LPREngine instance
        /// </summary>
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources of LPREngine instance
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                //ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            if (hPlate != IntPtr.Zero)
            {
                DTKLPR4.LicensePlate_Destroy(this.hPlate);
                hPlate = IntPtr.Zero;
            }
        }  

        public DateTime DateTime
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(this.Timestamp).ToLocalTime();
            }
        }


        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int MultiByteToWideChar(int codepage, int flags, IntPtr utf8, int utf8len, StringBuilder buffer, int buflen);

        public static string Utf8PtrToString(IntPtr utf8)
        {
            int len = MultiByteToWideChar(65000, 0, utf8, -1, null, 0);
            if (len == 0) return "";
            var buf = new StringBuilder(len+1);
            len = MultiByteToWideChar(65000, 0, utf8, -1, buf, len + 1);
            return buf.ToString();
        }

        public string Text
        {
            get
            {
                IntPtr text_utf8 = Marshal.AllocHGlobal(50);
                DTKLPR4.LicensePlate_GetText(this.hPlate, text_utf8, 50);
                return Utf8PtrToString(text_utf8);
            }
        }

        public string CountryCode
        {
            get
            {
                StringBuilder builder = new StringBuilder(50);
                DTKLPR4.LicensePlate_GetCountryCode(this.hPlate, builder, 50);
                return builder.ToString();
            }
        }

        public int X
        {
            get
            {
                return DTKLPR4.LicensePlate_GetX(this.hPlate);
            }
        }

        public int Y
        {
            get
            {
                return DTKLPR4.LicensePlate_GetY(this.hPlate);
            }
        }

        public int Width
        {
            get
            {
                return DTKLPR4.LicensePlate_GetWidth(this.hPlate);
            }
        }

        public int Height
        {
            get
            {
                return DTKLPR4.LicensePlate_GetHeight(this.hPlate);
            }
        }

        public int Confidence
        {
            get
            {
                return DTKLPR4.LicensePlate_GetConfidence(this.hPlate);
            }
        }

        public int Zone
        {
            get
            {
                return DTKLPR4.LicensePlate_GetZone(this.hPlate);
            }
        }

        public string ZoneName
        {
            get
            {
                 int size = DTKLPR4.LicensePlate_GetZoneName(this.hPlate, null, 0);
                 StringBuilder builder = new StringBuilder(size);
                 DTKLPR4.LicensePlate_GetZoneName(this.hPlate, builder, size);
                 return builder.ToString();
            }
        }

        public int Direction
        {
            get
            {
                return DTKLPR4.LicensePlate_GetDirection(this.hPlate);
            }
        }

        public long Timestamp
        {
            get
            {
                return DTKLPR4.LicensePlate_GetTimestamp(this.hPlate);
            }
        }

        public long Id
        {
            get
            {
                return DTKLPR4.LicensePlate_GetId(this.hPlate);
            }
        }

        public long CustomData
        {
            get
            {
                return DTKLPR4.LicensePlate_GetCustomData(this.hPlate);
            }
        }

        public Image Image
        {
            get
            {
                IntPtr pBuffer;
                int width;
                int height;
                int stride;
                DTKLPR4.LicensePlate_GetImageBuffer(this.hPlate, out pBuffer, out width, out height, out stride);
                if (pBuffer == IntPtr.Zero)
                    return null;
                Bitmap bmp = DTKLPR4.CreateBitmapFromBuffer(pBuffer, width, height, stride, PIXFMT.RGB24);
                DTKLPR4.LicensePlate_FreeImageBuffer(pBuffer);
                return bmp;
            }
        }

        public Image PlateImage
        {
            get
            {
                IntPtr pBuffer;
                int width;
                int height;
                int stride;
                DTKLPR4.LicensePlate_GetPlateImageBuffer(this.hPlate, out pBuffer, out width, out height, out stride);
                if (pBuffer == IntPtr.Zero)
                    return null;
                Bitmap bmp = DTKLPR4.CreateBitmapFromBuffer(pBuffer, width, height, stride, PIXFMT.RGB24);
                DTKLPR4.LicensePlate_FreeImageBuffer(pBuffer);
                return bmp;
            }
        }
    }

    /// <summary>
    /// LPREngine class
    /// </summary>
    public class LPREngine : IDisposable
    {
        internal IntPtr hEngine = IntPtr.Zero;

        private LicensePlateDetected callback = null;
        private DTKLPR4.LicensePlateDetectedNative navive_callback = null;

        /// <summary>
        /// Constructor of LPREngine instance
        /// </summary>
        public LPREngine(LPRParams parameters, bool bVideo, LicensePlateDetected callback)
        {
            this.callback = callback;
            this.navive_callback = new DTKLPR4.LicensePlateDetectedNative(OnLicensePlateDetectedNative);
            this.hEngine = DTKLPR4.LPREngine_Create(parameters.hParmas, bVideo,Marshal.GetFunctionPointerForDelegate(navive_callback));
        }

        /// <summary>
        /// Destructor of LPREngine instance
        /// </summary>
        ~LPREngine()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        /// <summary>
        /// Releases resources of LPREngine instance
        /// </summary>
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources of LPREngine instance
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                //ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            if (hEngine != IntPtr.Zero)
            {
                DTKLPR4.LPREngine_Destroy(this.hEngine);
                hEngine = IntPtr.Zero;
            }
        }  

        private void OnLicensePlateDetectedNative(IntPtr engine, IntPtr hPlate)
        {
            LicensePlate plate = new LicensePlate(hPlate);
            if (this.callback != null)
                this.callback(this, plate);
        }

        /// <summary>
        /// Return values:
        /// 0 - license is valid
        /// 1 - no license 
        /// 2 - channels limit exceeded (license is valid)
        /// </summary>
        public int IsLicensed
        {
            get
            {
                return DTKLPR4.LPREngine_IsLicensed(this.hEngine);
            }
        }

        /// <summary>
        /// Reads license plates from image file, the engine must be initialized for still image processing (bVideo parameters must be set to 'False' in LPR engine constructor).
        /// </summary>
        /// <param name="fileName">full path to image file (BMP, JPG, PNG)</param>
        /// <returns>List of LicensePlate objects</returns>
        public List<LicensePlate> ReadFromFile(string fileName)
        {
            List<LicensePlate> res = new List<LicensePlate>();
            IntPtr hResult = DTKLPR4.LPREngine_ReadFromFile(this.hEngine, fileName);
            int count = DTKLPR4.LPRResult_GetPlatesCount(hResult);
            for (int i=0; i<count; i++)
            {
                IntPtr hPlate = DTKLPR4.LPRResult_GetPlate(hResult, i);
                res.Add(new LicensePlate(hPlate));
            }
            DTKLPR4.LPRResult_Destroy(hResult);
            return res;
        }

        /// <summary>
        /// eads license plates from image buffer, the engine must be initialized for still image processing (bVideo parameters must be set to 'False' in LPR engine constructor).
        /// </summary>
        /// <param name="pBuffer">pointer to memory buffer</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <param name="stride">the size of a single image row in bytes</param>
        /// <param name="pixelFormat">1 - grayscale, 2 - RGB24, 4 - YUV420</param>
        /// <returns></returns
        public List<LicensePlate> ReadFromImageBuffer(IntPtr pBuffer, int width, int height, int stride, PIXFMT pixelFormat)
        {
            List<LicensePlate> res = new List<LicensePlate>();
            IntPtr hResult = DTKLPR4.LPREngine_ReadFromImageBuffer(this.hEngine, pBuffer, width, height, stride, pixelFormat);
            int count = DTKLPR4.LPRResult_GetPlatesCount(hResult);
            for (int i = 0; i < count; i++)
            {
                IntPtr hPlate = DTKLPR4.LPRResult_GetPlate(hResult, i);
                res.Add(new LicensePlate(hPlate));
            }
            return res;
        }

        /// <summary>
        /// Puts the video frame to processing queue, the engine must be initialized for video processing (bVideo parameters must be set to 'True' in LPR engine constructor).
        /// </summary>
        /// <param name="pBuffer">pointer to memory buffer</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <param name="stride">the size of a single image row in bytes</param>
        /// <param name="pixelFormat">1 - grayscale, 2 - RGB24, 4 - YUV420</param>
        /// <param name="frameID">custom parameter which can be used to identify video frame</param>
        /// <returns></returns>
        public int PutFrameImageBuffer(IntPtr pBuffer, int width, int height, int stride, PIXFMT pixelFormat, long timestamp, long customData)
        {
            return DTKLPR4.LPREngine_PutFrameImageBuffer(this.hEngine, pBuffer, width, height, stride, pixelFormat, timestamp, customData);
        }
    }

    /// <summary>
    /// VideoCapture class
    /// </summary>
    public class VideoCapture : IDisposable
    {
        internal IntPtr hVideoCapture = IntPtr.Zero;

        private FrameCaptured frameCapturedCallback = null;
        private CaptureError captureErrorCallback = null;
        private DTKLPR4.FrameCapturedNative navive_frameCapturedCallback = null;
        private DTKLPR4.CaptureErrorNative navive_captureErrorCallback = null;
        private object customObject;
        /// <summary>
        /// Constructor of LPREngine instance
        /// </summary>
        public VideoCapture(FrameCaptured frameCapturedCallback, CaptureError captureErrorCallback, object customObject)
        {
            this.frameCapturedCallback = frameCapturedCallback;
            this.captureErrorCallback = captureErrorCallback;
            this.navive_frameCapturedCallback = new DTKLPR4.FrameCapturedNative(OnFrameCapturedNative);
            this.navive_captureErrorCallback = new DTKLPR4.CaptureErrorNative(OnCaptureErrorNative);
            this.customObject = customObject;
        
            this.hVideoCapture = DTKLPR4.VideoCapture_Create(
                Marshal.GetFunctionPointerForDelegate(navive_frameCapturedCallback), 
                Marshal.GetFunctionPointerForDelegate(navive_captureErrorCallback), 
                IntPtr.Zero);
        }

        /// <summary>
        /// Destructor of LPREngine instance
        /// </summary>
        ~VideoCapture()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        /// <summary>
        /// Releases resources of LPREngine instance
        /// </summary>
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources of LPREngine instance
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                //ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            if (hVideoCapture != IntPtr.Zero)
            {
                DTKLPR4.VideoCapture_StopCapture(this.hVideoCapture);
                DTKLPR4.VideoCapture_Destroy(this.hVideoCapture);
                hVideoCapture = IntPtr.Zero;
            }
        }

        private void OnFrameCapturedNative(IntPtr hVideoCapture, IntPtr pBuffer, int width, int height, int stride, PIXFMT pixelFormat, long timestamp, IntPtr notUsed)
        {
            if (this.frameCapturedCallback != null)
                this.frameCapturedCallback(this, pBuffer, width, height, stride, pixelFormat, timestamp, this.customObject);
        }

        private void OnCaptureErrorNative(IntPtr hVideoCapture, ERR_CAPTURE errorCode, IntPtr notUsed)
        {
            if (this.captureErrorCallback != null)
                this.captureErrorCallback(this, errorCode, this.customObject);
        }

        public int StartCaptureFromFile(string fileName, int repeat_count)
        {
            return DTKLPR4.VideoCapture_StartCaptureFromFile(this.hVideoCapture, fileName, repeat_count);
        }

        public int StartCaptureFromIPCamera(string ipCameraURL)
        {
            return DTKLPR4.VideoCapture_StartCaptureFromIPCamera(this.hVideoCapture, ipCameraURL);
        }

        public int StartCaptureFromDevice(int deviceIndex, int captureWidth, int captureHeight)
        {
            return DTKLPR4.VideoCapture_StartCaptureFromDevice(this.hVideoCapture, deviceIndex, captureWidth, captureHeight);
        }

        public int StopCapture()
        {
            return DTKLPR4.VideoCapture_StopCapture(this.hVideoCapture);
        }
    }

}
