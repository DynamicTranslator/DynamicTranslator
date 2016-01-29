# DynamicTranslator 
[![Build Status](https://travis-ci.org/osoykan/DynamicTranslator.svg?branch=master)](https://travis-ci.org/osoykan/DynamicTranslator) [![Issue Stats](http://issuestats.com/github/osoykan/dynamictranslator/badge/issue?style=flat)](http://issuestats.com/github/osoykan/dynamictranslator) [![Issue Stats](http://issuestats.com/github/osoykan/dynamictranslator/badge/pr?style=flat)](http://issuestats.com/github/osoykan/dynamictranslator) [![Coverage Status](https://coveralls.io/repos/osoykan/DynamicTranslator/badge.svg?branch=master&service=github)](https://coveralls.io/github/osoykan/DynamicTranslator?branch=master) [![GitHub issues](https://img.shields.io/github/issues/osoykan/DynamicTranslator.svg)](https://github.com/osoykan/DynamicTranslator/issues) [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/osoykan/DynamicTranslator/master/LICENSE)
<a href="http://sourcebrowser.io/Browse/osoykan/DynamicTranslator"><img src="https://camo.githubusercontent.com/54520255524a72a04b0b20191e804f1360f85ab2/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f42726f7773652d536f757263652d677265656e2e737667" alt="Source Browser" data-canonical-src="https://img.shields.io/badge/Browse-Source-green.svg" style="max-width:100%;"></a>
<a href="https://scan.coverity.com/projects/osoykan-dynamictranslator">
  <img alt="Coverity Scan Build Status"
       src="https://scan.coverity.com/projects/7147/badge.svg"/>
</a>

While you are reading a pdf or something, DynamicTranslator detects selected texts instantly, translates them according to your language choice. 

###Latest News
DynamicTranslator detects your selected text where the current window/application you on and translates including words/sentences immediately.

#### *Instantly detect text implemented.
#### *Google Translate added.
#### *Language Detection implemented


###In Turkish
Bilindiği gibi bazı sözlükler bize api sağlamıyor bu yüzden bu proje tamamiyle windows ortamında sağlıklı ve en hızlı şekilde anlık çeviriyi pop-up yaklaşımıyla çözmeye yönelik bir amaçla yazılmıştır. Bulunduğunuz pencere/pdf veya herhangi birşeyde gezinirken mouse ile seçtiğiniz text'i algılayıp sonrasında, sırasıyla Google Translate, Tureng, Yandex, SesliSozluk'e gidip bulduğu anlamları bir araya getirip size windows notification olarak sunmaktadır.

Seçilen metnin dilini algılar.

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
