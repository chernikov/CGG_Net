# OpenAI GPT-5 Models Reference

> Source: [developers.openai.com/api/docs/models](https://developers.openai.com/api/docs/models)  
> Last updated: February 2026

---

## GPT-5 nano (`gpt-5-nano`)

**Fastest, most cost-efficient version of GPT-5.**

### Характеристики

| Параметр             | Значення                  |
|----------------------|---------------------------|
| Context window       | 400,000 tokens            |
| Max output tokens    | 128,000 tokens            |
| Knowledge cutoff     | May 31, 2024              |
| Reasoning tokens     | Supported                 |

### Ціна (per 1M tokens)

| Тип              | Ціна    |
|------------------|---------|
| Input            | $0.05   |
| Cached input     | $0.005  |
| Output           | $0.40   |

### Модальності

| Тип    | Підтримка        |
|--------|------------------|
| Text   | Input + Output   |
| Image  | Input only       |
| Audio  | Not supported    |
| Video  | Not supported    |

### Endpoints

- `v1/chat/completions` ✅
- `v1/responses` ✅
- `v1/batch` ✅
- `v1/fine-tuning` ❌
- `v1/embeddings` ✅

### Features

- Streaming ✅
- Function calling ✅
- Structured outputs ✅
- Fine-tuning ❌
- Distillation ❌

### Tools (Responses API)

- Web search ✅
- File search ✅
- Image generation ✅
- Code interpreter ✅
- Computer use ❌
- MCP ✅

### Коли використовувати

- Класифікація та категоризація
- Короткі резюме/summary
- Прості structured output задачі
- Висока частота запитів, де важлива ціна
- Фонові/batch операції

### Snapshots

| Alias       | Snapshot              |
|-------------|-----------------------|
| gpt-5-nano  | gpt-5-nano-2025-08-07 |

### Rate Limits

| Tier   | RPM    | TPM           | TPD                |
|--------|--------|---------------|--------------------|
| Tier 1 | 500    | 200,000       | 2,000,000          |
| Tier 2 | 5,000  | 2,000,000     | 20,000,000         |
| Tier 3 | 5,000  | 4,000,000     | 40,000,000         |
| Tier 4 | 10,000 | 10,000,000    | 1,000,000,000      |
| Tier 5 | 30,000 | 180,000,000   | 15,000,000,000     |

---

## GPT-5 mini (`gpt-5-mini`)

**A faster, cost-efficient version of GPT-5 for well-defined tasks.**

### Характеристики

| Параметр             | Значення                  |
|----------------------|---------------------------|
| Context window       | 400,000 tokens            |
| Max output tokens    | 128,000 tokens            |
| Knowledge cutoff     | May 31, 2024              |
| Reasoning tokens     | Supported                 |

### Ціна (per 1M tokens)

| Тип              | Ціна    |
|------------------|---------|
| Input            | $0.25   |
| Cached input     | $0.025  |
| Output           | $2.00   |

### Модальності

| Тип    | Підтримка        |
|--------|------------------|
| Text   | Input + Output   |
| Image  | Input only       |
| Audio  | Not supported    |
| Video  | Not supported    |

### Endpoints

- `v1/chat/completions` ✅
- `v1/responses` ✅
- `v1/batch` ✅
- `v1/fine-tuning` ❌
- `v1/embeddings` ✅

### Features

- Streaming ✅
- Function calling ✅
- Structured outputs ✅
- Fine-tuning ❌
- Distillation ❌

### Tools (Responses API)

- Web search ✅
- File search ✅
- Image generation ❌
- Code interpreter ✅
- Computer use ❌
- MCP ✅

### Коли використовувати

- AI-рекомендації для опитувань (CGG основний use case)
- Аналіз відповідей користувача
- Генерація структурованих звітів
- Задачі де потрібна логіка складніша за nano, але не потрібен повний GPT-5

### Snapshots

| Alias       | Snapshot              |
|-------------|-----------------------|
| gpt-5-mini  | gpt-5-mini-2025-08-07 |

### Rate Limits

| Tier   | RPM    | TPM           | TPD                |
|--------|--------|---------------|--------------------|
| Tier 1 | 500    | 500,000       | 5,000,000          |
| Tier 2 | 5,000  | 2,000,000     | 20,000,000         |
| Tier 3 | 5,000  | 4,000,000     | 40,000,000         |
| Tier 4 | 10,000 | 10,000,000    | 1,000,000,000      |
| Tier 5 | 30,000 | 180,000,000   | 15,000,000,000     |

---

## Порівняння моделей

| Модель      | Input $/1M | Output $/1M | Context   | Використання в CGG                    |
|-------------|------------|-------------|-----------|---------------------------------------|
| GPT-5       | $1.25      | $10.00      | 400k      | -                                     |
| GPT-5 mini  | $0.25      | $2.00       | 400k      | AI-рекомендації, аналіз опитувань     |
| GPT-5 nano  | $0.05      | $0.40       | 400k      | Класифікація, короткі summary, batch  |

---

## Правила вибору моделі в CGG

```
NanoModel (gpt-5-nano):
  ✅ Класифікація відповіді
  ✅ Визначення мови/теми
  ✅ Короткий summary (1-2 речення)
  ✅ Batch-обробка
  ✅ Валідація/фільтрація тексту

MiniModel (gpt-5-mini):
  ✅ Генерація AI-рекомендацій для кар'єри
  ✅ Аналіз результатів опитування
  ✅ Structured output з деталізацією
  ✅ Multi-step reasoning задачі
  ✅ Відповіді що потребують якості
```

---

**Офіційна документація:**
- [gpt-5-nano](https://developers.openai.com/api/docs/models/gpt-5-nano)
- [gpt-5-mini](https://developers.openai.com/api/docs/models/gpt-5-mini)
- [Всі моделі](https://developers.openai.com/api/docs/models)
- [Ціни](https://developers.openai.com/api/docs/pricing)
