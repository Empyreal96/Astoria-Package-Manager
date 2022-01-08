using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astoria_Package_Manager
{
    /// <summary>
    /// This is a library containing Android API levels
    /// </summary>
    class AndroidAPIs
    {
        public static string minSDKLevel { get; set; }
        public static string targetSDKLevel { get; set; }
        public static string PermissionsRequested { get; set; }
        public static void CheckminAPILevel(string str)
        {
            if (str == "1")
            {
                minSDKLevel = "Android 1.0";
            }
            if (str == "2")
            {
                minSDKLevel = "Android 1.1";
            }
            if (str == "3")
            {
                minSDKLevel = "Android 1.5";
            }
            if (str == "4")
            {
                minSDKLevel = "Android 1.6";
            }
            if (str == "5")
            {
                minSDKLevel = "Android 2.0";
            }
            if (str == "6")
            {
                minSDKLevel = "Android 2.0.1";
            }
            if (str == "7")
            {
                minSDKLevel = "Android 2.1";
            }
            if (str == "8")
            {
                minSDKLevel = "Android 2.2";
            }
            if (str == "9")
            {
                minSDKLevel = "Android 2.3.0 - 2.3.2";
            }
            if (str == "10")
            {
                minSDKLevel = "Android 2.3.3 - 2.3.7";
            }
            if (str == "11")
            {
                minSDKLevel = "Android 3.0";
            }
            if (str == "12")
            {
                minSDKLevel = "Android 3.1";
            }
            if (str == "13")
            {
                minSDKLevel = "Android 3.2";
            }
            if (str == "14")
            {
                minSDKLevel = "Android 4.0.1 - 4.0.2";
            }
            if (str == "15")
            {
                minSDKLevel = "Android 4.0.3 - 4.0.4";
            }
            if (str == "16")
            {
                minSDKLevel = "Android 4.1";
            }
            if (str == "17")
            {
                minSDKLevel = "Android 4.2";
            }
            if (str == "18")
            {
                minSDKLevel = "Android 4.3";
            }
            if (str == "19")
            {
                minSDKLevel = "Android 4.4";
            }
            if (str == "20")
            {
                minSDKLevel = "Android 5.0";
            }
            if (str == "21")
            {
                minSDKLevel = "Android 5.0";
            }
            if (str == "22")
            {
                minSDKLevel = "Android 5.1";
            }
            if (str == "23")
            {
                minSDKLevel = "Android 6";
            }
            if (str == "24")
            {
                minSDKLevel = "Android 7.0";
            }
            if (str == "25")
            {
                minSDKLevel = "Android 7.1";
                       }
            if (str == "26")
            {
                minSDKLevel = "Android 8.0";
            }
            if (str == "27")
            {
                minSDKLevel = "Android 8.1";
                       }
            if (str == "28")
            {
                minSDKLevel = "Android 9";
            }
            if (str == "29")
            {
                minSDKLevel = "Android 10";
                       }
            if (str == "30")
            {
                minSDKLevel = "Android 11";
            }
            if (str == "31")
            {
                minSDKLevel = "Android 12";
                       }
            if (str == "32")
            {
                minSDKLevel = "Android 13";
            }

        }
        public static void CheckmaxAPILevel(string str)
        {
            if (str == "1")
            {
                targetSDKLevel = "Android 1.0";
            }
            if (str == "2")
            {
                targetSDKLevel = "Android 1.1";
            }
            if (str == "3")
            {
                targetSDKLevel = "Android 1.5";
            }
            if (str == "4")
            {
                targetSDKLevel = "Android 1.6";
            }
            if (str == "5")
            {
                targetSDKLevel = "Android 2.0";
            }
            if (str == "6")
            {
                targetSDKLevel = "Android 2.0.1";
            }
            if (str == "7")
            {
                targetSDKLevel = "Android 2.1";
            }
            if (str == "8")
            {
                targetSDKLevel = "Android 2.2";
            }
            if (str == "9")
            {
                targetSDKLevel = "Android 2.3.0 - 2.3.2";
            }
            if (str == "10")
            {
                targetSDKLevel = "Android 2.3.3 - 2.3.7";
            }
            if (str == "11")
            {
                targetSDKLevel = "Android 3.0";
            }
            if (str == "12")
            {
                targetSDKLevel = "Android 3.1";
            }
            if (str == "13")
            {
                targetSDKLevel = "Android 3.2";
            }
            if (str == "14")
            {
                targetSDKLevel = "Android 4.0.1 - 4.0.2";
            }
            if (str == "15")
            {
                targetSDKLevel = "Android 4.0.3 - 4.0.4";
            }
            if (str == "16")
            {
                targetSDKLevel = "Android 4.1";
            }
            if (str == "17")
            {
                targetSDKLevel = "Android 4.2";
            }
            if (str == "18")
            {
                targetSDKLevel = "Android 4.3";
            }
            if (str == "19")
            {
                targetSDKLevel = "Android 4.4";
            }
            if (str == "20")
            {
                targetSDKLevel = "Android 5.0";
            }
            if (str == "21")
            {
                targetSDKLevel = "Android 5.0";
            }
            if (str == "22")
            {
                targetSDKLevel = "Android 5.1";
            }
            if (str == "23")
            {
                targetSDKLevel = "Android 6";
            }
            if (str == "24")
            {
                targetSDKLevel = "Android 7.0";
            }
            if (str == "25")
            {
                targetSDKLevel = "Android 7.1";
            }
            if (str == "26")
            {
                targetSDKLevel = "Android 8.0";
            }
            if (str == "27")
            {
                targetSDKLevel = "Android 8.1";
            }
            if (str == "28")
            {
                targetSDKLevel = "Android 9";
            }
            if (str == "29")
            {
                targetSDKLevel = "Android 10";
            }
            if (str == "30")
            {
                targetSDKLevel = "Android 11";
            }
            if (str == "31")
            {
                targetSDKLevel = "Android 12";
            }
            if (str == "32")
            {
                targetSDKLevel = "Android 13";
            }
        }


        public static void CheckPermissions(string str)
        {
            PermissionsRequested = "";
            if (str.Contains("android.permission.SEND_SMS") == true)
            {
                PermissionsRequested += "Send SMS/MMS\n";
            }
            if (str.Contains("android.permission.CALL_PHONE") == true)
            {
                PermissionsRequested += "Make Phone Calls\n";
            }
            if (str.Contains("android.permission.READ_CONTACTS") == true)
            {
                PermissionsRequested += "Read Contact List\n";
            }
            if (str.Contains("android.permission.WRITE_CONTACTS") == true)
            {
                PermissionsRequested += "Modify Contact List\n";
            }
            if (str.Contains("android.permission.READ_CALENDAR") == true)
            {
                PermissionsRequested += "Read Calendar Data\n";
            }
            if (str.Contains("android.permission.WRITE_CALENDAR") == true)
            {
                PermissionsRequested += "Modify Calendar Data\n";
            }
            if (str.Contains("android.permission.READ_USER_DICTIONARY") == true)
            {
                PermissionsRequested += "Read User Dictionary\n";
            }
            if (str.Contains("android.permission.WRITE_USER_DICTIONARY") == true)
            {
                PermissionsRequested += "Modify User Dictionary\n";
            }
            if (str.Contains("android.permission.ACCESS_FINE_LOCATION") == true)
            {
                PermissionsRequested += "Location Access (Fine)\n";
            }
            if (str.Contains("android.permission.ACCESS_COARSE_LOCATION") == true)
            {
                PermissionsRequested += "Location Access (Coarse)\n";
            }
            if (str.Contains("android.permission.CAMERA") == true)
            {
                PermissionsRequested += "Access Camera Hardware\n";
            }
            if (str.Contains("android.permission.ACCESS_NETWORK_STATE") == true)
            {
                PermissionsRequested += "Access Network Info\n";
            }
            if (str.Contains("android.permission.ACCESS_WIFI_STATE") == true)
            {
                PermissionsRequested += "Access Wi-Fi Info\n";
            }
            if (str.Contains("android.permission.BLUETOOTH") == true)
            {
                PermissionsRequested += "Bluetooth Capabilities\n";
            }
            if (str.Contains("android.permission.FLASHLIGHT") == true)
            {
                PermissionsRequested += "Use Camera Flash\n";
            }
            if (str.Contains("android.permission.INTERNET") == true)
            {
                PermissionsRequested += "Requires Internet Access\n";
            }
            if (str.Contains("android.permission.RECORD_AUDIO") == true)
            {
                PermissionsRequested += "Access Microphone\n";
            }
            if (str.Contains("android.permission.MODIFY_AUDIO_SETTINGS") == true)
            {
                PermissionsRequested += "Modify Audio Settings\n";
            }
            if (str.Contains("android.permission.WAKE_LOCK") == true)
            {
                PermissionsRequested += "Prevent Screen Sleep\n";
            }
            if (str.Contains("android.permission.VIBRATE") == true)
            {
                PermissionsRequested += "Vibration\n";
            }
            if (str.Contains("android.permission.WRITE_SETTINGS") == true)
            {
                PermissionsRequested += "Modify System Settings\n";
            }
            if (str.Contains("android.permission.REQUEST_INSTALL_PACKAGES") == true)
            {
                PermissionsRequested += "Install Packages\n";
            }
            if (str.Contains("android.permission.READ_SYNC_SETTINGS") == true)
            {
                PermissionsRequested += "Read Account Sync Settings\n";
            }
            if (str.Contains("android.permission.WRITE_SYNC_SETTINGS") == true)
            {
                PermissionsRequested += "Modify Account Sync Settings\n";
            }
            if (str.Contains("android.permission.SET_PROCESS_LIMIT") == true)
            {
                PermissionsRequested += "Modify Process Limit\n";
            }
            if (str.Contains("android.permission.SET_ALWAYS_FINISH") == true)
            {

            }
            if (str.Contains("android.permission.DUMP") == true)
            {

            }
            if (str.Contains("android.permission.SIGNAL_PERSISTENT_PROCESSES") == true)
            {

            }
            if (str.Contains("android.permission.KILL_BACKGROUND_PROCESSES") == true)
            {
                PermissionsRequested += "Kill Background Applications\n";
            }
            if (str.Contains("android.permission.FORCE_BACK") == true)
            {

            }
            if (str.Contains("android.permission.BATTERY_STATS") == true)
            {
                PermissionsRequested += "Read Battery Status\n";
            }
            if (str.Contains("android.permission.INTERNAL_SYSTEM_WINDOW") == true)
            {

            }
            if (str.Contains("android.permission.INJECT_EVENTS") == true)
            {

            }
            if (str.Contains("android.permission.RETRIEVE_WINDOW_CONTENT") == true)
            {

            }
            if (str.Contains("android.permission.SET_ACTIVITY_WATCHER") == true)
            {

            }
            if (str.Contains("android.permission.READ_INPUT_STATE") == true)
            {

            }
            if (str.Contains("android.permission.SET_ORIENTATION") == true)
            {

            }
            if (str.Contains("android.permission.INSTALL_PACKAGES") == true)
            {
                PermissionsRequested += "Install Packages\n";
            }
            if (str.Contains("android.permission.CLEAR_APP_USER_DATA") == true)
            {
                PermissionsRequested += "Clear Application User Data\n";
            }
            if (str.Contains("android.permission.DELETE_CACHE_FILES") == true)
            {
                PermissionsRequested += "Modify Cache Files\n";
            }
            if (str.Contains("android.permission.DELETE_PACKAGES") == true)
            {
                PermissionsRequested += "Remove Installed Packages\n";
            }
            if (str.Contains("android.permission.ACCESS_SURFACE_FLINGER") == true)
            {

            }
            if (str.Contains("android.permission.READ_FRAME_BUFFER") == true)
            {
                PermissionsRequested += "Read Framebuffer\n";
            }
            if (str.Contains("android.permission.DEVICE_POWER") == true)
            {
                PermissionsRequested += "Control Power Options\n";
            }
            if (str.Contains("android.permission.INSTALL_LOCATION_PROVIDER") == true)
            {

            }
            if (str.Contains("android.permission.BACKUP") == true)
            {
                PermissionsRequested += "Backup Data";
            }
            if (str.Contains("android.permission.FORCE_STOP_PACKAGES") == true)
            {

            }
            if (str.Contains("android.permission.STOP_APP_SWITCHES") == true)
            {

            }
            if (str.Contains("android.permission.ACCESS_CONTENT_PROVIDERS_EXTERNALLY") == true)
            {

            }
            if (str.Contains("android.permission.GRANT_REVOKE_PERMISSIONS") == true)
            {
                PermissionsRequested += "Modify Permissions\n";
            }
            if (str.Contains("android.permission.SET_KEYBOARD_LAYOUT") == true)
            {

            }
            if (str.Contains("android.permission.GET_DETAILED_TASKS") == true)
            {

            }
            if (str.Contains("android.permission.SET_SCREEN_COMPATIBILITY") == true)
            {

            }
            if (str.Contains("android.permission.READ_EXTERNAL_STORAGE") == true)
            {
                PermissionsRequested += "Read External Storage\n";
            }
            if (str.Contains("android.permission.WRITE_EXTERNAL_STORAGE") == true)
            {
                PermissionsRequested += "Write External Storage\n";
            }
            if (str.Contains("android.permission.INTERACT_ACROSS_USERS") == true)
            {

            }
            if (str.Contains("android.permission.INTERACT_ACROSS_USERS_FULL") == true)
            {

            }
            if (str.Contains("android.permission.MANAGE_USERS") == true)
            {
                PermissionsRequested += "Modify Device Users\n";
            }
            if (str.Contains("android.permission.BLUETOOTH_STACK") == true)
            {

            }
            if (str.Contains("android.permission.GET_ACCOUNTS") == true)
            {
                PermissionsRequested += "Get User Accounts\n";
            }

        }
    }

}
