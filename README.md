# DynamicTranslator
Dynamically catch your copy (Control + C) instruction and it translate including words.

### Project Information and The Goal
This project provides translation words or sentences while reading and working and any needed time. So, I'm using this while PDF Ebook reading mostly.Project is small but very useful (at least me :)) I hope this useful for you.

If you contribute, I'm glad.

C# , WPF

This is a view while translating, the translating is showing via toast notification for translated words.
![alt-tag](http://i57.tinypic.com/r9mrdg.png)

### Using
The solution have a app.config like below. I don't any UI implementation yet, but you can then let's contribute !

```
<appSettings>
    <add key="LeftOffset" value="500" />
    <add key="TopOffset" value="15" />
    <add key="ApiKey" value="" />
    <add key="SearchableCharacterLimit" value="100" />
    <add key="MaxNotifications" value="4" />
    <add key="FromLanguage" value="English" />
    <add key="ToLanguage" value="Turkish" />
    <add key="ClientSettingsProvider.ServiceUri" value="" />
</appSettings>
  ```
  
  And now; You should go Yandex Api console and get a Translate Api Key and paste it 
  ```
  <add key="ApiKey" value="YOURTRANSLATEAPIKEY" /> 
  ```
  this section.
  
  And you can change any language which allowing by YANDEX Translate system on here.
  ```
    <add key="FromLanguage" value="English" />
    <add key="ToLanguage" value="Turkish" />
  ```
  
  
