# DynamicTranslator 
While you are reading a pdf or something, when you press the "Control + C"(it is not necessary anymore, DynamicTranslator detects selected texts instantly, but still working),  "Dynamic Translator" immediately detect the your word and translates it.

###Latest News
You are not using Control + C (it's optional) no longer and DynamicTranslator detects your selected text where the current window/application you on and translates including words/sentences immediately.

#### *Instantly detect text implemented.
#### *Google Translate added.



###In Turkish
Bilindiği gibi bazı sözlükler bize api sağlamıyor bu yüzden bu proje tamamiyle windows ortamında sağlıklı ve en hızlı şekilde anlık çeviriyi pop-up yaklaşımıyla çözmeye yönelik bir amaçla yazılmıştır. Bulunduğunuz pencere/pdf veya herhangi birşeyde gezinirken mouse ile seçtiğiniz text'i algılayıp sonrasında, sırasıyla Google Translate, Tureng, Yandex, SesliSozluk'e gidip bulduğu anlamları bir araya getirip size windows notification olarak sunmaktadır.

###Başvurulan sözlükler
        
##### Tureng
##### Yandex
##### Sesli Sozluk
##### Google Translate
        

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
#İmplemented C# 6.0 and .NET 4.6
