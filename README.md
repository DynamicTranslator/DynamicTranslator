# DynamicTranslator
While you are reading a pdf or something, when you press the "Control + C",  "Dynamic Translator" immediately detect the your word and translates it.

###Latest News
You are not using Control + C (it's optional) no longer and DynamicTranslator detects your selected text where the current window/application you on and translates including words/sentences immediately.

#### *Instantly detect text implemented.



###In Turkish
Bilindiği gibi Tureng sözlük bize bir api sağlamamakta, yeni yapılan geliştirmeyle, sözcük ilk başta Tureng'e gidip sitede sözcüğü aratıp çıkan sonuçları Toast notification'da göstermektedir. Orda bulamazsa Yandex'e gidip arama yapıyor.Sonucu toast notification'da gösteriyor. 

Proje tamamiyle windows ortamında sağlıklı ve en hızlı şekilde anlık çeviriyi pop-up yaklaşımıyla çözmeye yönelik bir amaçla yazılmıştır.

###Başvurulan sözlükler
        
##### Tureng
##### Yandex
##### Sesli Sozluk (yapım aşamasında)
        

### Project Information and The Goal
This project provides translation words or sentences while reading and working and any needed time. So, I'm using this while PDF Ebook reading mostly.Project is small but very useful (at least me :)) I hope this useful for you.

C# , WPF

This is a view while translating, the translating is showing via toast notification for translated words.
![alt-tag](http://i57.tinypic.com/r9mrdg.png)

### Using
It has an app.config like below. I didn't do any UI implementation yet, i think it's not necessary :), but you can do, then let's contribute !

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
  
  And now; You should go Yandex Api console and get a Translate Api Key and paste it  this section.
  ```
  <add key="ApiKey" value="YOURTRANSLATEAPIKEY" /> 
  ```
  
  And you can change any language which allowing by YANDEX Translate system on here.
  ```
    <add key="FromLanguage" value="English" />
    <add key="ToLanguage" value="Turkish" />
  ```
  
  
