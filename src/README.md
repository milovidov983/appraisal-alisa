﻿# appraisal-alisa

# Описание структуры проекта

Ядро приложения это набор стратегий которые обрабатывают пользовательский ввод в зависимости от типа пользовательского сообщения. Переходы между стратегиями описаны в `AliceAppraisal.Static.DefaultTransitions` в этом классе описаны переходы по умолчанию. Так же возможны переходы не описанные в данной карте, в них пользователь попадает в зависимости от условий предыдущего шага и его текущего ввода.  

Основной обработчик находится в файле `AliceAppraisal.Application.MainHandelr` в нем выполняется получение подходящей стратегии, запуск стратегии, формирование ответа, либо обработка непредвиденного запроса, его так же обрабатывает выбранная стратегия. Так же в данном классе обрабатываются исключения.


# Roadmap

- Проговаривать распознанный интент
- Какие есть типы коробок передач/двигателей и т д
- запоминать текущее ожидание что бы подсказывать:  кажется это не коробка передач, попробуйте еще раз или попросите у меня подсказку
- ((под)?скажи)? текущий шаг (оценки)? 
- Повтор команды - протестировать
- команда оценить
- вернутся на предыдущий шаг
- запоминать если пользователь уже видел подсказку и не выводить инфо о том что за бот
- дать возможность оценить если данных хватает


# Complete

- что ты умеешь
- интерфейс для без экранных (выберите/назовите) + киа рио 2017 нашлось два поколения в1 в2


# Deploy

yc serverless function version create \
--function-name=scharp1 \
--runtime dotnetcore31-preview \
--entrypoint AliceAppraisal.Controllers.Handler \
--memory 128m \ # Объем RAM.
--execution-timeout 3s \
--source-path ./sources.zip

