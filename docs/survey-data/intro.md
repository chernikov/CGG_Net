Джерело https://docs.google.com/document/d/1LP4K72LdmP_3FYBvUvo7R-xoqMqb93BRkEfqokfj_Y4/edit?tab=t.0#heading=h.d28ugz0tdri


Пробний тест для батьків: «Моя кар’єрна орієнтація»

1) Survey spec (Parent • Trial)
Name: Пробний тест для батьків: «Моя кар’єрна орієнтація»
 Type: single-select per question (3 options)
 Languages: UA / EN / HI
 Notes: We assign short, stable codes to each option to keep payloads tiny and scoring simple.
Q1 — Мрія на старті кар’єри / Early dream role
uk: Ким ви мріяли стати на початку свого професійного шляху?


en: At the start of your career, what did you dream of becoming?


hi: अपने करियर की शुरुआत में आप क्या बनना चाहते थे?
 Options (codes):


Q1_HELP — Тим, хто допомагає людям (лікар, вчитель, психолог…) / Helping others / लोगों की मदद करने वाला


Q1_BUILD — Тим, хто створює і будує (інженер, підприємець, майстер) / Building & creating / बनाने-निर्माण वाला


Q1_IDEA — Тим, хто досліджує і розвиває ідеї (науковець, дизайнер, аналітик) / Ideas & research / विचार-अनुसंधान


Q2 — Що найважливіше у професії зараз / What matters most now
Options:
Q2_STAB — Стабільність і дохід / Stability & income / स्थिरता-आय


Q2_GROW — Можливість розвитку й самореалізації / Growth & self-realization / विकास-आत्मसिद्धि


Q2_BAL — Баланс між роботою та особистим життям / Work-life balance / काम-जीवन संतुलन


Q3 — Що вас найбільше мотивує / What motivates you most
Options:
Q3_RECOG — Визнання та повага / Recognition & respect / मान-सम्मान


Q3_CREAT — Творчість і цікаві завдання / Creativity & interesting tasks / रचनात्मकता


Q3_ADV — Кар’єрний ріст і нові можливості / Career growth & new opportunities / उन्नति-अवसर


Q4 — Відкритість до змін / Openness to change
Options:
Q4_YES — Так, повністю / Yes, fully / हाँ, पूरी तरह


Q4_MAYBE — Можливо, якщо буде потреба / Maybe, if needed / शायद, आवश्यकता पर


Q4_NO — Ні, хочу залишитись у своїй сфері / No, prefer same field / नहीं, उसी क्षेत्र में


Q5 — Як уявляєте себе за 2 роки / Self-image in 2 years
Options:
Q5_LEAD — Керую командою чи проектом / Leading team/project / नेतृत्व


Q5_STAB — Працюю у стабільній сфері з передбачуваним доходом / Stable role / स्थिर भूमिका


Q5_NEW — Реалізовую себе у новій чи більш творчій професії / New/creative field / नया-रचनात्मक



2) JSON config (drop-in, matches your style)


{
  "systemPrompt": "You are a practical career mentor and family-oriented psychologist. Your task is to interpret a short parent survey in clear, simple language that any average adult can easily understand. Avoid complex psychological terms and abstract labels. Use everyday language. The result must be easy to read in under 30 seconds and help the parent feel understood. Gently show why deeper career guidance (for them and their child) could be useful. Each field must be one paragraph, written in a warm and supportive tone, no more than 900 characters total. Output must match the interface language (default: English). Follow the required JSON structure exactly and return only a valid JSON object with no extra text.",

  "userPromptTemplate": "LANG={{lang}}\nROLE=parent\nSURVEY=parent_orientation_trial\n\nInterpret the selected answer codes below in simple, clear language. Do not repeat the options. Do not over-analyze. Focus on clarity, usefulness, and emotional safety. Make the parent feel understood and gently curious about going deeper into career guidance.\n\n<ANSWERS>\n{{answerCodesJson}}\n</ANSWERS>\n\nHint Mapping:\nQ1_HELP → caring/helping orientation\nQ1_BUILD → practical/creating orientation\nQ1_IDEA → thinking/idea-oriented\nQ2_STAB → security focus\nQ2_GROW → growth focus\nQ2_BAL → balance focus\nQ3_RECOG → need for recognition\nQ3_CREAT → need for creativity\nQ3_ADV → advancement focus\nQ4_YES → high openness\nQ4_MAYBE → moderate openness\nQ4_NO → low openness\nQ5_LEAD → leadership direction\nQ5_STAB → security direction\nQ5_NEW → novelty direction\n\nReturn only a valid JSON object following the required schema. Each field must be a single paragraph."
}


Output
Your Career Pattern:
Ви людина, яка любить розуміти, як усе працює, і постійно розвиватись. Вам важливо не стояти на місці й бачити реальний рух вперед.
What Drives You:
Вас мотивує можливість рости, брати на себе більше відповідальності та впливати на результат. Вам подобається відчувати, що ви рухаєтесь до чогось більшого, а не просто “працюєте заради стабільності”. Ви готові до змін, якщо бачите в них сенс і перспективу.
How This Affects Your Child:
Ви можете несвідомо очікувати від дитини амбіційності та прагнення до розвитку. Іноді це надихає, а іноді може створювати тиск, якщо темп дитини інший. Ваш підхід може формувати в дитини сміливість пробувати нове — якщо він подається через підтримку, а не вимогу.
What Could Help Next:
Корисно краще зрозуміти власні мотиви й порівняти їх із природними схильностями дитини. Це допоможе уникнути зайвих очікувань і побудувати більш гармонійний кар’єрний шлях для всієї родини.



















English as default output language


Finalized polished tri-lingual microcopy (UA / EN / HI) ready for front-end use


Compact structure for easy developer drop-in
{
  "$schema": "./schemas/step.schema.json",
  "step": 1,
  "name": "parent_orientation_trial",
  "title": {
    "uk": "Пробний тест для батьків: «Моя кар’єрна орієнтація»",
    "en": "Parent Trial: My Career Orientation",
    "hi": "अभिभावक परीक्षण: मेरा करियर अभिमुखीकरण"
  },
  "description": {
    "uk": "Короткий опитник, щоб зрозуміти вашу професійну орієнтацію, мотиви та очікування.",
    "en": "A short survey to understand your career orientation, motives, and expectations.",
    "hi": "आपके करियर अभिमुखीकरण, प्रेरणाओं और अपेक्षाओं को समझने के लिए एक संक्षिप्त सर्वे।"
  },
  "questions": [
    {
      "id": "q1_early_dream",
      "type": "select",
      "text": {
        "uk": "Ким ви мріяли стати на початку свого професійного шляху?",
        "en": "At the beginning of your career journey, what did you dream of becoming?",
        "hi": "अपने करियर की शुरुआत में आप क्या बनना चाहते थे?"
      },
      "options": {
        "uk": [
          "Тим, хто допомагає людям (лікар, вчитель, психолог тощо)",
          "Тим, хто створює і будує (інженер, підприємець, майстер)",
          "Тим, хто досліджує і розвиває ідеї (науковець, дизайнер, аналітик)"
        ],
        "en": [
          "Someone who helps people (doctor, teacher, psychologist, etc.)",
          "Someone who builds and creates (engineer, entrepreneur, craftsman)",
          "Someone who explores and develops ideas (scientist, designer, analyst)"
        ],
        "hi": [
          "लोगों की मदद करने वाला व्यक्ति (डॉक्टर, शिक्षक, मनोवैज्ञानिक आदि)",
          "बनाने और निर्माण करने वाला व्यक्ति (इंजीनियर, उद्यमी, कारीगर)",
          "विचारों का अन्वेषण और विकास करने वाला व्यक्ति (वैज्ञानिक, डिजाइनर, विश्लेषक)"
        ]
      },
      "optionCodes": ["Q1_HELP", "Q1_BUILD", "Q1_IDEA"]
    },
    {
      "id": "q2_now_matters",
      "type": "select",
      "text": {
        "uk": "Що для вас найважливіше у професії зараз?",
        "en": "What matters most to you in your career right now?",
        "hi": "इस समय आपके करियर में आपके लिए सबसे महत्वपूर्ण क्या है?"
      },
      "options": {
        "uk": [
          "Стабільність і дохід",
          "Можливість розвитку й самореалізації",
          "Баланс між роботою та особистим життям"
        ],
        "en": [
          "Stability and income",
          "Opportunity for growth and self-realization",
          "Balance between work and personal life"
        ],
        "hi": [
          "स्थिरता और आय",
          "विकास और आत्म-सिद्धि का अवसर",
          "काम और निजी जीवन के बीच संतुलन"
        ]
      },
      "optionCodes": ["Q2_STAB", "Q2_GROW", "Q2_BAL"]
    },
    {
      "id": "q3_motivation",
      "type": "select",
      "text": {
        "uk": "Що вас найбільше мотивує у роботі?",
        "en": "What motivates you most at work?",
        "hi": "काम में आपको सबसे ज़्यादा क्या प्रेरित करता है?"
      },
      "options": {
        "uk": [
          "Визнання та повага",
          "Творчість і цікаві завдання",
          "Кар’єрний ріст і нові можливості"
        ],
        "en": [
          "Recognition and respect",
          "Creativity and interesting tasks",
          "Career growth and new opportunities"
        ],
        "hi": [
          "मान्यता और सम्मान",
          "रचनात्मकता और रोचक कार्य",
          "करियर में उन्नति और नए अवसर"
        ]
      },
      "optionCodes": ["Q3_RECOG", "Q3_CREAT", "Q3_ADV"]
    },
    {
      "id": "q4_change_openness",
      "type": "select",
      "text": {
        "uk": "Чи відкриті ви до зміни професії або сфери діяльності?",
        "en": "Are you open to changing your profession or field of work?",
        "hi": "क्या आप अपने पेशे या कार्य क्षेत्र को बदलने के लिए खुले हैं?"
      },
      "options": {
        "uk": [
          "Так, повністю",
          "Можливо, якщо буде потреба",
          "Ні, хочу залишитись у своїй сфері"
        ],
        "en": ["Yes, completely", "Maybe, if necessary", "No, I prefer to stay in my field"],
        "hi": ["हाँ, पूरी तरह", "शायद, यदि आवश्यकता हो", "नहीं, मैं अपने क्षेत्र में रहना पसंद करता हूँ"]
      },
      "optionCodes": ["Q4_YES", "Q4_MAYBE", "Q4_NO"]
    },
    {
      "id": "q5_self_in_2y",
      "type": "select",
      "text": {
        "uk": "Як ви уявляєте себе на роботі через два роки?",
        "en": "How do you imagine yourself at work in two years?",
        "hi": "आप खुद को दो साल बाद अपने काम में कैसे देखते हैं?"
      },
      "options": {
        "uk": [
          "Керую командою чи проектом",
          "Працюю у стабільній сфері з передбачуваним доходом",
          "Реалізовую себе у новій або більш творчій професії"
        ],
        "en": [
          "Leading a team or project",
          "Working in a stable field with predictable income",
          "Expressing myself in a new or more creative profession"
        ],
        "hi": [
          "एक टीम या परियोजना का नेतृत्व कर रहा/रही हूँ",
          "एक स्थिर क्षेत्र में काम कर रहा/रही हूँ जहाँ आय पूर्वानुमेय है",
          "एक नए या अधिक रचनात्मक पेशे में खुद को अभिव्यक्त कर रहा/रही हूँ"
        ]
      },
      "optionCodes": ["Q5_LEAD", "Q5_STAB", "Q5_NEW"]
    }
  ],

  "systemPrompt": "You are an experienced career psychologist and educator. Based on the parent’s responses, write a concise, warm, and analytical interpretation. Your goals: (1) identify their current career orientation type (e.g., helping, creative, analytical, stable); (2) describe their inner motives, values, and implicit patterns (stability, creativity, autonomy, recognition); (3) reflect how this mindset may influence parenting and family values; (4) add a short psychological micro-interpretation (temperament, motivation, or thinking style); and (5) end with a gentle reflective tip for growth or balance. Avoid diagnostic or clinical terms. Keep the response within 1000 characters. Output must match the user interface language (default: English). Format the response exactly as:\n\nCareer Type: …\nPsychological Core: …\nGrowth Zone: …\nReflective Tip: …",

  "userPromptTemplate": "LANG={{lang}}\nROLE=parent\nSURVEY=parent_orientation_trial\n\nWrite the answer in the interface language (default English). Interpret the selections below and extract implicit motives, not just explicit choices.\n\n<ANSWERS>\n{{answerCodesJson}}\n</ANSWERS>\n\nHint Mapping:\nQ1_HELP→helping orientation; Q1_BUILD→builder/entrepreneurial; Q1_IDEA→analytic/creative.\nQ2_STAB→stability; Q2_GROW→growth/self-realization; Q2_BAL→balance.\nQ3_RECOG→recognition; Q3_CREAT→creativity; Q3_ADV→advancement.\nQ4_YES→high openness; Q4_MAYBE→moderate openness; Q4_NO→low openness.\nQ5_LEAD→leadership; Q5_STAB→security; Q5_NEW→novelty/creativity.\n\nReturn only the four labeled lines in the exact structure defined in systemPrompt.",

  "model": {
    "provider": "openai",
    "name": "gpt-5-mini",
    "reasoning": { "effort": "medium" },
    "text": { "verbosity": "medium" }
  },
  "outputFormat": {
    "type": "structured",
    "mode": "short",
    "formatFile": "/formats/parent-orientation-analysis.json"
  }
}


























NOT Actual
Ким ви мріяли стати на початку свого професійного шляху?


Тим, хто допомагає людям (лікар, вчитель, психолог тощо)


Тим, хто створює і будує (інженер, підприємець, майстер)


Тим, хто досліджує і розвиває ідеї (науковець, дизайнер, аналітик)


Що для вас найважливіше у професії зараз?


Стабільність і дохід


Можливість розвитку й самореалізації


Баланс між роботою та особистим життям


Що вас найбільше мотивує у роботі?


Визнання та повага


Творчість і цікаві завдання


Кар’єрний ріст і нові можливості


Чи відкриті ви до зміни професії/сфери?


Так, повністю


Можливо, якщо буде потреба


Ні, хочу залишитись у своїй сфері


Як ви уявляєте себе в роботі через 5 років?


Керую командою чи проектом


Працюю у стабільній сфері з передбачуваним доходом


Реалізовую себе у новій чи більш творчій професії

Промт:Ти — досвідчений психолог, педагог і кар’єрний консультант.
Твоя мета — дати коротку, глибоку та доброзичливу аналітичну рекомендацію для батьків, які проходять пробне тестування «Моя кар’єрна орієнтація».
Завдання
На основі відповідей користувача опиши його актуальну кар’єрну спрямованість, внутрішні мотиви й несвідомі очікування від професійного життя.
Виявляй не лише усвідомлені вибори (що людина відповіла), а й імпліцитні патерни — наприклад, схильність до стабільності, творчого пошуку, потреби в автономії чи визнанні.
Розкрий, як цей тип кар’єрної орієнтації впливає на виховання дитини, стиль підтримки й моделі цінностей у родині.
Додай невелику психологічну мікроінтерпретацію (1–2 речення) — що цей вибір може означати у термінах темпераменту, стилю мислення чи мотивації.
Завершуй короткою рефлексійною порадою — як можна розвивати власну гнучкість, самоусвідомлення чи баланс між професійним і особистим.
	
Формат результату:
Обсяг: до 1000 символів
Тон: теплий, аналітичний, з відтінком підтримки
Структура відповіді:
Тип орієнтації (опис професійного типу, напр. «допомагаючий», «творчий», «аналітичний» тощо)
Психологічне ядро — внутрішні мотиви, цінності, ключові ресурси
Зони розвитку — що може допомогти реалізувати потенціал
Рефлексивна порада — м’яка, натхненна рекомендація - чи варто до обговорення???



