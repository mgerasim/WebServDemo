WebServDemo
===========



Реализовать HTTP сервер в виде консольного приложения. Без использования WCF, ASP.NET.
Порт, по которому будут приниматься запросы, должен читаться из конфигурационного файла. Информацию о запросах необходимо писать на консоль.
1) На любой запрос из браузера отдавать "Hello world!"
2) Реализовать гостевую книгу с поддержкой двух функций:
"  при запросе GET /Guestbook/ отдавать все записи;
"	при запросе POST /Guestbook/ добавлять запись в гостевую книгу, принимая параметры user и message.
Сообщения хранить в XML файле.
3) Второй вариант хранения данных гостевой книги. 
Сохранять сообщения в базу SQLite организовав там таблицы Users и Messages. 
(Управление вариантами хранения через конфигурационный файл.)
4) При запросе /Proxy/ с параметром url, HTTP сервер должен возвращать содержимое страницы расположенной по указанному url.

запросы GET/POST обрабатываются функциями класса HttpServer
запрос					Функция обработчик класса HttpServer		Описание
GET /					handleRoot									Главная страница
GET /new				handleGETnew								Форма заполнения новой записи в Гостевую книгу
POST /guestbook			handlePOSTguestbook							Встака новой записи
GET /guestbook			handleGETguestbook							Просмотр Гостевой книги
GET /proxy				handleGETproxy								Возвращает содержимое страницы расположенной по указанному в параметре url
																	вариант использования http://127.0.0.1:8080/proxy?url=http://127.0.0.1:8080/guestnotes
						handleDefault								Обрабатывает все остальные запросы
						
Для поддержки нового запроса например GET /test необходимо реализовать в классе HttpServer функцию handleGETtest.

КОНФИГУРАЦИЯ СЕРВЕРА:
Конфигурационный файл распологается в том же каталоге, что и приложение. Например если приложение имеет имя WebServerDemo.exe, то конфигурационный файл называется WebServerDemo.exe.config
Настройки имеет следующую структуру
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <appSettings>
        <add key="Port" value="8080" />
        <add key="XmlFile" value="0" />
        <add key="Sqlite" value="1" />
    </appSettings>
</configuration>

Port=8080 сервер прослушывает указанный порт (8080)
XmlFile=1 указывает на то чтобы записи Гостевой книги сохранять в XML файлы. Данные файлы сохраняются в той же директории что сервер-приложение.
Sqlite=1 указывает на то чтобы записи Гостевой книгги сохранять в SQLite базе данных. Файл базы данных распологается в одной директории с приложением под именени guestnotes.db

																	


