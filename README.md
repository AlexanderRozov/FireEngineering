# FireEngineering

Расчётное ядро массы HFC-227ea по СП 485.1311500.2020.

## Тесты

`tests/FireEngineering.Tests` — 14 xUnit-тестов:

- объём: бокс, цилиндр, призма, вычет
- негерметичность \(K_2\) и предел по таблице Г.16
- масса HFC-227ea, поправка \(K_3\), отказ неизвестного ГОТВ
- общая масса \(M_g = 1{,}05(M_r + M_{tr} + M_b n)\)

```bash
dotnet test tests/FireEngineering.Tests/FireEngineering.Tests.csproj
```

CI: `.github/workflows/tests.yml`.

Таблицы СП — Draft, перед проектом нужна сверка специалистом.
