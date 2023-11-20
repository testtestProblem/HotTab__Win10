# Make font icon  
* Download font icon  
Reference - https://icofont.com/  
* Find out unicode in svg file 
```XML
<glyph glyph-name="ui-bluetooth" horiz-adv-x="1000" unicode="&#xec42;" d="M594 348c54-55 108-108 162-162 32-32 32-57-1-90-76-76-151-152-227-227-30-30-73-24-87 12-4 10-4 21-4 32 0 83 0 166 0 249v28c-8-8-14-12-18-16l-102-102c-30-30-60-34-83-10-23 24-20 52 10 82l178 177c22 22 22 33 0 55-61 62-123 123-184 185-30 30-22 74 16 87 22 7 40 0 55-15 36-36 71-71 106-107 6-5 12-10 20-18 1 9 2 14 2 20 0 81 0 161 0 242 0 7-1 14-2 21-3 24 7 42 27 53 21 10 41 8 58-9 10-9 19-18 29-28 70-70 140-140 211-211 27-27 27-54 0-81-49-50-99-99-148-148-6-6-12-12-18-19z m-55 100c37 37 75 74 107 106-35 35-72 72-107 108v-214z m0-418c38 39 76 76 109 109l-109 110v-219z" />
```
Or, changing to HTML for easyer search what you want  
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/0683dd86-1df5-442b-8196-c65a9656e9e4)

* Put ttf into assets  
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/73c07b2a-4b03-46de-bfcd-04b9028ff3c8)

* ttf propertice set copy always
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/16204720-2d8d-4239-b874-e15dd2a88bf5)

* Add this into app.xmal
Attention - Filename should defined  
```XAML
<Application
    x:Class="HotTab_Win10.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:HotTab_Win10">

    <Application.Resources>
        <FontFamily x:Key="CustomIconFont">Assets/icofont.ttf#icofont
        </FontFamily>
    </Application.Resources>

</Application>
```


* Add icon font into MainPage.xaml
```
        <Button x:Name="btn_webCamRear" Content="&#xeecf;" FontFamily="{StaticResource ResourceKey=CustomIconFont}" Margin="28,178,0,0" VerticalAlignment="Top" Width="45" Height="35" FontSize="25" Click="btn_webCamRear_Click">
        </Button>
```


