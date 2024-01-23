// WMIO2.h

#define WMIO2_API __declspec(dllexport)

extern "C" WMIO2_API bool ModeOpen(UINT uiMode);
extern "C" WMIO2_API bool ModeClose();
extern "C" WMIO2_API bool SetDevice(BYTE uiValue);
extern "C" WMIO2_API bool GetDevice(PBYTE puiValue);
extern "C" WMIO2_API bool SetDevice2(BYTE uiValue);
extern "C" WMIO2_API bool GetDevice2(PBYTE puiValue);
extern "C" WMIO2_API bool SetDevice3(BYTE uiValue);
extern "C" WMIO2_API bool GetDevice3(PBYTE puiValue);
extern "C" WMIO2_API bool GetSMBIOSInfo(PCHAR pstName, PCHAR pstValue);
extern "C" WMIO2_API bool GetBattery1SpecificInfo(UINT uiCommand, PUINT puiValue);
extern "C" WMIO2_API bool GetBattery2SpecificInfo(UINT uiCommand, PUINT puiValue);
extern "C" WMIO2_API bool GetCPUTemperature(PBYTE puiValue);
extern "C" WMIO2_API bool SetCPUPL1(BYTE uiValue);
extern "C" WMIO2_API bool GetCPUPL1(PBYTE uiValue);
extern "C" WMIO2_API bool GetDockingStatus(PBYTE puiValue);
