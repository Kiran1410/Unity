from unity_frameWork_setting import UnityFrameWorkSetting

if __name__ == '__main__':

    assets = UnityFrameWorkSetting()
    assets.update_main_mm_files()
    assets.update_header_files()
    assets.update_build_setting()
    assets.bridge_File_Target_Public()
