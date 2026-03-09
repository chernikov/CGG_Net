⭐ Варіант 4 — «Профіль розвитку: батьківський внесок»
🪄 Варіант 4 — «Оберіть Шлях Майбутнього Чарівника»
Опис
Міні гайд для батьків — як це працює

За допомогою нашого додатку ви можете:
	•	краще зрозуміти свою дитину — її приховані й явні бажання, здібності та мотиви,
	•	надати своє бачення її сильних сторін, зон розвитку та можливого шляху, заповнивши короткий профіль у системі,
	•	співставити свій погляд із баченням дитини (та за потреби — інших важливих дорослих). На цій основі система сформує оптимальний прогноз її кар’єрного розвитку.

Все, що потрібно від вас: заповнити опитувальник, дозволити дитині пройти свої завдання — і отримати спільний, узгоджений результат!

🪄 1. «Дзеркало Твого Чарівного Потенціалу»

🪄 3. «Кристал Навичок: Відкрий Свій Справжній Дар»
1) Survey spec (Parent → Child Talents)
Name: Батьківське бачення талантів дитини / Parent View of Child’s Talents
 Type: Mixed (select, multiselect, textarea)
 Audience: Parent (on child’s record)
 Validation: sensible min/max; gentle tone; gender-neutral.
 Notes: Where the question suggests “свої думки / own words”, we support Other with a conditional text field.
Q1 — Тип дитини (single-select + optional “Other”)
uk: Як би ви описали тип дитини?
 (Обиріть найближче, або “Інше”)


en: How would you describe your child’s general type? (Choose closest, or “Other”)


hi: आप अपने बच्चे के स्वभाव/प्रकृति का वर्णन कैसे करेंगे? (सबसे नज़दीकी चुनें, या “अन्य”)
 Options (codes):
 CALM спокійна / Calm / शांत
 EMO емоційна / Emotional / भावुक
 ACTIVE активна / Active / सक्रिय
 WITHDRAWN замкнена / Reserved / संकोची
 OTHER свої думки / Other / अन्य
 → If OTHER, show Q1_OTHER (short text, max 80 chars)


Q2 — Улюблені заняття / Hobbies (textarea)
Examples hint displayed; max 400 chars.


Q3 — Професійні мрії / Dreams (textarea)
If none, allow “Немає / None”.


Q4 — Що важливо для підтримки / Support needs (textarea)
Prompt examples in helper text; max 400 chars.


Q5 — Чи маєте сформовану думку про професію? (textarea)
Short, neutral; max 280 chars.


Q6 — Сімейні професії (textarea)
Ask how family traditions may influence choice; max 400 chars.


Q7 — Таланти: знання/уміння/навички (textarea, require ≥3 comma-separated items)
Helper: “Назвіть щонайменше три (через кому) / At least three, comma-separated.”


Q8 — Зони розвитку / Needs support (textarea, 2–3 items)
Helper: “Назвіть 2–3 напрямки / Name 2–3 areas.”


Q9 — Умови щасливої професії (multiselect + optional “Other”)
uk: Які якості або умови в майбутній професії ви вважаєте найважливішими для щасливого життя вашої дитини?


en: Which job qualities/conditions will matter most for your child’s happy life?


hi: भविष्य की नौकरी में कौन-से गुण/शर्तें आपके बच्चे की खुशी के लिए सबसे महत्वपूर्ण होंगी?
 Options (codes; up to 3 selections):
 STABILITY стабільність / Stability / स्थिरता
 CREATIVITY творчість / Creativity / रचनात्मकता
 PRESTIGE престиж / Prestige / प्रतिष्ठा
 HELPING допомога іншим / Helping others / दूसरों की मदद
 INCOME високий дохід / High income / उच्च आय
 FLEXTIME вільний графік / Flexible schedule / लचीला समय
 OTHER свої думки / Other / अन्य
 → If OTHER, show Q9_OTHER (short text, max 80 chars)


Q10 — Улюблений герой дитини (textarea)
Why attractive? max 300 chars.


Q11 — Нелюбимий персонаж / Антигерой (textarea)
What’s disliked? max 300 chars.


Q12 — Ким із родичів захоплюється (textarea)
Why inspiring? max 300 chars.


Q13 — На кого не хотіла б бути схожа/схожий (textarea)
Which traits/behaviors disliked? max 300 chars.


Q14 — Персонаж, схожий на себе (textarea)
Why similar? max 300 chars.


Q15 — Додаткові важливі характеристики (textarea, optional)
Anything else; max 400 chars.


Q16 — Локальний запит (“Мій запит до системи”) (textarea)
Helper examples visible; max 300 chars.


This field is prioritized in AI response if present.



2) JSON config (drop-in; matches your schema style)


{
  "$schema": "./schemas/format.schema.json",
  "description": "Long format for parent survey about child - returns comprehensive career matches and child-focused insights, plus prompts for generation",
  "format": {
    "type": "object",
    "properties": {
      "matches": {
        "type": "array",
        "minItems": 3,
        "maxItems": 5,
        "description": "Top 3-5 career matches for the child",
        "items": {
          "type": "object",
          "properties": {
            "rank": {
              "type": "number",
              "minimum": 1,
              "maximum": 5,
              "description": "Ranking position"
            },
            "title": {
              "type": "string",
              "description": "Profession name (child-focused)"
            },
            "matchPercentage": {
              "type": "number",
              "minimum": 0,
              "maximum": 100,
              "description": "Match percentage (0-100)"
            },
            "why": {
              "type": "string",
              "description": "Why this direction may fit the child (single paragraph)"
            },
            "strongSkills": {
              "type": "array",
              "items": { "type": "string" },
              "minItems": 3,
              "description": "Child strengths/resources observed"
            },
            "skillsToImprove": {
              "type": "array",
              "items": { "type": "string" },
              "minItems": 2,
              "description": "Skills/competencies to develop"
            },
            "salaryRange": {
              "type": "object",
              "properties": {
                "junior": { "type": "string", "description": "Entry/junior level salary range" },
                "mid": { "type": "string", "description": "Mid level salary range" },
                "senior": { "type": "string", "description": "Senior level salary range" }
              },
              "required": ["junior", "mid", "senior"],
              "additionalProperties": false
            },
            "education": {
              "type": "object",
              "properties": {
                "offline": {
                  "type": "array",
                  "items": { "type": "string" },
                  "minItems": 1,
                  "description": "Offline education paths"
                },
                "online": {
                  "type": "array",
                  "items": { "type": "string" },
                  "minItems": 1,
                  "description": "Online learning resources"
                }
              },
              "required": ["offline", "online"],
              "additionalProperties": false
            },
            "nextSteps": {
              "type": "array",
              "items": { "type": "string" },
              "minItems": 3,
              "description": "Actionable next steps for parent/child"
            },
            "psychologicalProfile": {
              "type": "array",
              "items": { "type": "string" },
              "minItems": 3,
              "description": "Child-focused psychological profile cues relevant for this career"
            }
          },
          "required": [
            "rank",
            "title",
            "matchPercentage",
            "why",
            "strongSkills",
            "skillsToImprove",
            "salaryRange",
            "education",
            "nextSteps",
            "psychologicalProfile"
          ],
          "additionalProperties": false
        }
      },
      "overallChildProfile": {
        "type": "string",
        "description": "Overall child-focused profile summary (single paragraph)"
      },
      "parentGuidance": {
        "type": "object",
        "properties": {
          "howToSupport": {
            "type": "array",
            "items": { "type": "string" },
            "minItems": 3,
            "description": "How parents can support (emotional/educational/organizational)"
          },
          "whatToAvoid": {
            "type": "array",
            "items": { "type": "string" },
            "minItems": 2,
            "description": "What parents should avoid (to keep safe environment)"
          }
        },
        "required": ["howToSupport", "whatToAvoid"],
        "additionalProperties": false
      },
      "systemPrompt": {
        "type": "string",
        "description": "System prompt used to generate the output"
      },
      "userPromptTemplate": {
        "type": "string",
        "description": "User prompt template with placeholders used to inject inputs"
      }
    },
    "required": ["matches", "overallChildProfile", "parentGuidance", "systemPrompt", "userPromptTemplate"],
    "additionalProperties": false
  },
  "instructions": "Respond only with a valid JSON object in the following format, with no text before or after, and no newlines inside string values",
  "example": {
    "matches": [
      {
        "rank": 1,
        "title": "Керівник проєктів / Організатор процесів",
        "matchPercentage": 82,
        "why": "Цей напрям може підійти вашій дитині, якщо їй важливо бачити результат і впливати на процес, а також брати відповідальність і рухати справи вперед у команді.",
        "strongSkills": ["Ініціативність", "Відповідальність", "Комунікабельність", "Орієнтація на результат"],
        "skillsToImprove": ["Делегування", "Гнучкість у складних ситуаціях", "Стратегічне мислення"],
        "salaryRange": {
          "junior": "$40 000–$60 000",
          "mid": "$65 000–$95 000",
          "senior": "$100 000–$140 000"
        },
        "education": {
          "offline": ["Менеджмент", "Бізнес-адміністрування", "Управління проєктами"],
          "online": ["Agile/Scrum курси", "Google Project Management", "Курси з лідерства"]
        },
        "nextSteps": ["Доручати невеликі організаційні завдання вдома або в школі", "Залучати до проєктної діяльності (гуртки, шкільні ініціативи)", "Тренувати планування: короткі списки справ і підсумки дня"],
        "psychologicalProfile": ["Природно бере ініціативу", "Цінує результат і вплив", "Може бути вимогливою до себе", "Краще працює, коли є зрозумілі правила"]
      },
      {
        "rank": 2,
        "title": "Бізнес-аналітик / Дослідник процесів",
        "matchPercentage": 78,
        "why": "Підійде, якщо дитина любить логіку, ставить запитання і прагне розуміти, як усе влаштовано, щоб покращувати процеси та домовленості.",
        "strongSkills": ["Аналітичне мислення", "Спостережливість", "Структурованість"],
        "skillsToImprove": ["Презентаційні навички", "Впевненість у висловленні думок", "Робота з невизначеністю"],
        "salaryRange": {
          "junior": "$35 000–$55 000",
          "mid": "$60 000–$90 000",
          "senior": "$95 000–$130 000"
        },
        "education": {
          "offline": ["Економіка", "ІТ-напрями", "Бізнес-аналіз"],
          "online": ["Курси з бізнес-аналізу", "Основи аналітики даних", "Курси системного мислення"]
        },
        "nextSteps": ["Заохочувати задачі на аналіз і порівняння", "Вчити структурувати думки (причина–наслідок–висновок)", "Пробувати мініпроєкти: описати проблему й запропонувати рішення"],
        "psychologicalProfile": ["Любить ясність і логіку", "Шукає сенс у деталях", "Схильна довго обдумувати рішення", "Потребує безпечного простору для помилок"]
      },
      {
        "rank": 3,
        "title": "Підприємець / Творець власних проєктів",
        "matchPercentage": 75,
        "why": "Може підійти, якщо дитина має внутрішній драйв створювати щось своє, експериментувати та проявляти самостійність, а також хоче впливати на результат.",
        "strongSkills": ["Самостійність", "Гнучкість", "Внутрішня мотивація"],
        "skillsToImprove": ["Фінансова грамотність", "Командна взаємодія", "Довгострокове планування"],
        "salaryRange": {
          "junior": "$0–$50 000",
          "mid": "$50 000–$150 000",
          "senior": "$150 000+"
        },
        "education": {
          "offline": ["Підприємництво", "Інкубатори стартапів", "Бізнес-школи"],
          "online": ["Курси з підприємництва", "Основи фінансів", "Стартап-програми"]
        },
        "nextSteps": ["Підтримувати маленькі ініціативи дитини без надконтролю", "Дати простір для мініпроєктів (шкільних або домашніх)", "Разом навчати основам бюджету: витрати–доходи–ціль"],
        "psychologicalProfile": ["Орієнтується на свободу і самостійність", "Любить пробувати нове", "Може швидко приймати рішення", "Потребує підтримки, а не тиску"]
      }
    ],
    "overallChildProfile": "Ваша дитина має потенціал до відповідальності, розвитку та впливу; їй може бути комфортно там, де є простір для зростання, створення або організації, а також безпечний простір для експериментів і помилок.",
    "parentGuidance": {
      "howToSupport": ["Підтримуйте проби: маленькі кроки і регулярність важливіші за ідеальний результат", "Домовляйтесь про правила і свободу в межах цих правил (рамка + вибір)", "Хваліть за зусилля і прогрес, а не лише за перемоги"],
      "whatToAvoid": ["Не порівнюйте з іншими дітьми як спосіб мотивації", "Не забирайте відповідальність повністю — краще навчати і підстраховувати"]
    },
    "systemPrompt": "You are an expert career counselor and child–parent dynamics specialist. You analyze parent survey answers about a child, plus optional child survey results and observations if provided. Use all provided inputs for reasoning, but DO NOT reveal any private text that is not visible to the parent role. Write in a warm, practical tone for the child (use 'ти/твої' when the interface language is Ukrainian). Avoid diagnoses or clinical labeling. If there are signs of self-harm, bullying, or danger, include a gentle safety message encouraging reaching out to a trusted adult or professional. Produce 3–5 career matches with clear reasons, strengths, growth skills, education paths, salary ranges, and next steps. Then produce an overall child profile and parent guidance. Output MUST be valid JSON only, exactly matching the provided schema; no extra keys; no newline characters inside string values.",
    "userPromptTemplate": "LANG={{lang}} ROLE=parent_about_child SURVEY=parent_view_child_talents_v1 CHILD_ID={{child_id}} Return JSON only matching schema. Inputs: <PARENT_SURVEY_CODES>{{answerCodesJson}}</PARENT_SURVEY_CODES> <PARENT_SURVEY_TEXT>{{freeTextJson}}</PARENT_SURVEY_TEXT> <CHILD_SURVEY_RESULT optional>{{child_survey_result_json}}</CHILD_SURVEY_RESULT> <OBSERVATIONS_SUMMARY optional>{{observations_summary_json}}</OBSERVATIONS_SUMMARY> <LAST_RECO_SUMMARY optional>{{last_reco_summary_json}}</LAST_RECO_SUMMARY> Requirements: (1) Use simple, everyday language (no jargon) while staying psychologically insightful; (2) Compare parent vs child perspective if child data exists; (3) Provide actions that are realistic for 7–14 days; (4) Keep salary ranges region-appropriate if region provided, otherwise use broad ranges; (5) Never quote private observation texts; only refer to them as generalized themes; (6) If q16_local_request is present, tailor nextSteps and parentGuidance to it."
  }
}
































{
  "$schema": "./schemas/step.schema.json",
  "step": 1,
  "name": "parent_view_child_talents_v1",
  "title": {
    "uk": "Анкета для батьків: Бачення талантів дитини",
    "en": "Parent Survey: Your View of the Child’s Talents",
    "hi": "अभिभावक सर्वे: बच्चे की प्रतिभाओं पर आपका दृष्टिकोण"
  },
  "description": {
    "uk": "Поділіться своїм баченням сильних сторін, інтересів і умов, за яких дитина розкриватиме потенціал.",
    "en": "Share your view of the child’s strengths, interests, and the conditions where they thrive.",
    "hi": "बच्चे की खूबियों, रुचियों और अनुकूल परिस्थितियों पर अपना दृष्टिकोण साझा करें।"
  },
  "questions": [
    {
      "id": "q1_child_type",
      "type": "select",
      "text": {
        "uk": "Як би ви описали тип дитини? (Обиріть найближче або «Інше»)",
        "en": "How would you describe your child’s general type? (Choose the closest, or “Other”)",
        "hi": "आप अपने बच्चे के स्वभाव/प्रकृति का वर्णन कैसे करेंगे? (सबसे नज़दीकी चुनें, या “अन्य”)"
      },
      "options": {
        "uk": ["Спокійна", "Емоційна", "Активна", "Замкнена", "Інше"],
        "en": ["Calm", "Emotional", "Active", "Reserved", "Other"],
        "hi": ["शांत", "भावुक", "सक्रिय", "संकोची", "अन्य"]
      },
      "optionCodes": ["CALM", "EMO", "ACTIVE", "WITHDRAWN", "OTHER"]
    },
    {
      "id": "q1_other",
      "type": "input",
      "text": {
        "uk": "Опишіть своїми словами",
        "en": "Describe in your own words",
        "hi": "अपने शब्दों में बताइए"
      },
      "visibleIf": { "q1_child_type": "OTHER" },
      "maxLength": 80
    },
    {
      "id": "q2_hobbies",
      "type": "textarea",
      "text": {
        "uk": "Улюблені заняття чи хобі дитини, що можуть стати частиною майбутнього:",
        "en": "Your child’s favorite activities or hobbies that could be part of their future:",
        "hi": "बच्चे की पसंदीदा गतिविधियाँ/शौक जो भविष्य का हिस्सा बन सकते हैं:"
      },
      "maxLength": 400
    },
    {
      "id": "q3_dreams",
      "type": "textarea",
      "text": {
        "uk": "Чи має дитина професійні мрії або інтереси? Якщо так — які?",
        "en": "Does your child have professional dreams or interests? If yes, which?",
        "hi": "क्या बच्चे के पास कोई पेशेवर सपने/रुचियाँ हैं? यदि हाँ, कौन-सी?"
      },
      "maxLength": 400
    },
    {
      "id": "q4_support",
      "type": "textarea",
      "text": {
        "uk": "Що важливо для підтримки дитини у виборі майбутнього?",
        "en": "What do you consider important in supporting your child’s future choices?",
        "hi": "बच्चे के भविष्य के चुनाव में समर्थन के लिए आप क्या महत्वपूर्ण मानते हैं?"
      },
      "maxLength": 400
    },
    {
      "id": "q5_parent_opinion",
      "type": "textarea",
      "text": {
        "uk": "Чи маєте сформовану думку, яку професію повинна обрати ваша дитина?",
        "en": "Do you have a formed opinion about what profession your child should choose?",
        "hi": "क्या आपके मन में बच्चे के लिए कोई तय पेशा है?"
      },
      "maxLength": 280
    },
    {
      "id": "q6_family_traditions",
      "type": "textarea",
      "text": {
        "uk": "Чи є у вашій родині «династії» професій? Як це може вплинути на вибір дитини?",
        "en": "Are there professions that run in your family? Could this influence your child’s choice?",
        "hi": "क्या आपके परिवार में कुछ पेशे पीढ़ियों से चले आ रहे हैं? क्या इसका बच्चे के चयन पर प्रभाव हो सकता है?"
      },
      "maxLength": 400
    },
    {
      "id": "q7_talents_three",
      "type": "textarea",
      "text": {
        "uk": "У чому дитина талановита? Назвіть щонайменше три знання/уміння/навички (через кому).",
        "en": "Where is your child talented? List at least three knowledge/skills/abilities (comma-separated).",
        "hi": "बच्चा किसमें प्रतिभाशाली है? कम से कम तीन ज्ञान/कौशल/क्षमताएँ लिखें (कॉमा से अलग)।"
      },
      "placeholder": {
        "uk": "напр., аналітичне мислення, малювання, комунікація",
        "en": "e.g., analytical thinking, drawing, communication",
        "hi": "उदा., विश्लेषणात्मक सोच, ड्राइंग, संचार"
      },
      "minItemsHint": 3,
      "maxLength": 400
    },
    {
      "id": "q8_dev_areas",
      "type": "textarea",
      "text": {
        "uk": "У яких сферах дитині потрібен розвиток або підтримка? Назвіть 2–3 напрямки.",
        "en": "Where does your child need development or support? Name 2–3 areas.",
        "hi": "किस क्षेत्रों में बच्चे को विकास/सहयोग चाहिए? 2–3 क्षेत्र लिखें।"
      },
      "maxLength": 300
    },
    {
      "id": "q9_happy_job_conditions",
      "type": "multiselect",
      "maxSelections": 3,
      "text": {
        "uk": "Які умови в майбутній професії є найважливішими для щастя дитини?",
        "en": "Which conditions in a future job are most important for your child’s happiness?",
        "hi": "भविष्य की नौकरी में बच्चे की खुशी के लिए सबसे महत्वपूर्ण शर्तें कौन-सी हैं?"
      },
      "options": {
        "uk": ["Стабільність", "Творчість", "Престиж", "Допомога іншим", "Високий дохід", "Вільний графік", "Інше"],
        "en": ["Stability", "Creativity", "Prestige", "Helping others", "High income", "Flexible schedule", "Other"],
        "hi": ["स्थिरता", "रचनात्मकता", "प्रतिष्ठा", "दूसरों की मदद", "उच्च आय", "लचीला समय", "अन्य"]
      },
      "optionCodes": ["STABILITY", "CREATIVITY", "PRESTIGE", "HELPING", "INCOME", "FLEXTIME", "OTHER"]
    },
    {
      "id": "q9_other",
      "type": "input",
      "text": {
        "uk": "Інше — своїми словами",
        "en": "Other — in your own words",
        "hi": "अन्य — अपने शब्दों में"
      },
      "visibleIfContains": { "q9_happy_job_conditions": "OTHER" },
      "maxLength": 80
    },
    {
      "id": "q10_fav_character",
      "type": "textarea",
      "text": {
        "uk": "Якого улюбленого персонажа дитина найчастіше обирає чи згадує? Чому він приваблює дитину?",
        "en": "Which favorite character does your child choose or mention most? Why is it attractive to them?",
        "hi": "बच्चा किस पसंदीदा किरदार का सबसे अधिक ज़िक्र करता है? उसे वह क्यों आकर्षित करता है?"
      },
      "maxLength": 300
    },
    {
      "id": "q11_antihero",
      "type": "textarea",
      "text": {
        "uk": "Якого негативного персонажа (антигероя) дитина не сприймає? Що саме не подобається?",
        "en": "Which negative (antihero) character does your child dislike? What exactly is disliked?",
        "hi": "कौन-सा नकारात्मक (एंटीहीरो) किरदार बच्चे को पसंद नहीं? क्या नापसंद है?"
      },
      "maxLength": 300
    },
    {
      "id": "q12_admired_relative",
      "type": "textarea",
      "text": {
        "uk": "Ким із родичів дитина найбільше захоплюється? Чим ця людина надихає?",
        "en": "Which relative does your child admire most? Why are they inspiring?",
        "hi": "बच्चा किस रिश्तेदार से सबसे अधिक प्रभावित है? वे प्रेरणादायक क्यों हैं?"
      },
      "maxLength": 300
    },
    {
      "id": "q13_not_like_relative",
      "type": "textarea",
      "text": {
        "uk": "На кого з родичів дитина не хотіла б бути схожою? Які риси/поведінка не подобаються?",
        "en": "Which relative would your child not want to be like? Which traits/behaviors are disliked?",
        "hi": "किस रिश्तेदार जैसा बच्चा नहीं बनना चाहता? कौन-से गुण/व्यवहार नापसंद हैं?"
      },
      "maxLength": 300
    },
    {
      "id": "q14_lookalike_hero",
      "type": "textarea",
      "text": {
        "uk": "Якого героя або відомого персонажа дитина вважає найбільш схожим на себе? Чому?",
        "en": "Which hero or known character does your child consider most similar to themselves? Why?",
        "hi": "कौन-सा हीरो/प्रसिद्ध किरदार बच्चा अपने जैसा मानता है? क्यों?"
      },
      "maxLength": 300
    },
    {
      "id": "q15_extra",
      "type": "textarea",
      "text": {
        "uk": "Додаткові важливі характеристики (будь-що, що варто знати).",
        "en": "Additional important characteristics (anything we should know).",
        "hi": "अतिरिक्त महत्वपूर्ण बातें (जो हमें जाननी चाहिए)।"
      },
      "maxLength": 400,
      "optional": true
    },
    {
      "id": "q16_local_request",
      "type": "textarea",
      "text": {
        "uk": "Локальний запит: «Мій запит до системи» (напр.: самостійність, контакт, мотивація до навчання).",
        "en": "Local request: “My request to the system” (e.g., independence, better communication, study motivation).",
        "hi": "स्थानीय अनुरोध: “सिस्टम के लिए मेरा अनुरोध” (जैसे आत्मनिर्भरता, बेहतर संवाद, पढ़ाई की प्रेरणा)।"
      },
      "placeholder": {
        "uk": "Напр.: «Допомогти розвинути самостійність», «Як покращити контакт?»",
        "en": "E.g., “Help build independence”, “How to improve our communication?”",
        "hi": "उदा., “आत्मनिर्भरता बनानी है”, “संवाद कैसे बेहतर करें?”"
      },
      "maxLength": 300
    }
  ],
  "systemPrompt": "You are an expert in career counseling and child–parent dynamics. Analyze the parent’s survey on the child, together with any prior profiles and observations if provided. Your tasks: (1) identify what’s NEW or meaningful in the latest input; (2) explain how it AFFECTS the current career trajectory forecast (or does not); (3) give 1–2 concrete ACTIONS for parents (emotional, educational, organizational). Compare parent and child perspectives when possible. Keep a warm, motivating tone. Avoid diagnostic/clinical labeling. Keep the whole answer under 500 characters. Output must use the interface language (default: English). Format exactly:\n\nЩо нового: …\nВплив на прогноз: …\nЩо робити батькам: …",
  "userPromptTemplate": "LANG={{lang}}\nROLE=parent_about_child\nSURVEY=parent_view_child_talents_v1\nCHILD_ID={{child_id}}\n\nWrite the answer in LANG (default English). Use the exact three labeled lines from systemPrompt.\n\n<PARENT_SURVEY_CODES>\n{{answerCodesJson}}\n</PARENT_SURVEY_CODES>\n\n<PARENT_SURVEY_TEXT>\n{{freeTextJson}}\n</PARENT_SURVEY_TEXT>\n\n<CHILD_PROFILE optional>\n{{child_profile}}\n</CHILD_PROFILE>\n\n<PREVIOUS_PROFILE_JSON optional>\n{{previous_profile_json}}\n</PREVIOUS_PROFILE_JSON>\n\n<OBSERVATIONS_DELTA optional>\n{{observations_delta_json}}\n</OBSERVATIONS_DELTA>\n\n<LAST_RECOMMENDATION_SUMMARY optional>\n{{last_reco_summary}}\n</LAST_RECOMMENDATION_SUMMARY>\n\nPrioritize the Local Request (q16) if present, but still update the child’s trajectory forecast. Compare parent vs child data if present. Be specific, kind, and practical. ≤500 characters total. Return ONLY the three labeled lines.",
  "model": {
    "provider": "openai",
    "name": "gpt-5-mini",
    "reasoning": { "effort": "medium" },
    "text": { "verbosity": "medium" }
  },
  "outputFormat": {
    "type": "structured",
    "mode": "short",
    "formatFile": "/formats/parent-child-delta-brief.json"
  }
}






Not Actual
Як би ви описали тип дитини?
 (Виберіть найближче: спокійна / емоційна / активна / замкнена / свої думки)
Поділіться улюбленими заняттями чи хобі вашої дитини, які, на вашу думку, можуть стати частиною її майбутнього…?
Чи має ваша дитина професійні мрії або інтереси? Якщо так — які саме?
Що вам здається важливим для підтримки її у виборі майбутнього?
Чи є у вас сформована думка, яку професію повинна обрати ваша дитина?
Чи є у вашій родині професії, які передаються з покоління в покоління (наприклад, лікарі, вчителі, військові)? Як ви думаєте, чи може це вплинути на вибір вашої дитини?
В чому ваша дитина талановита? Які знання, вміння, навички має добре розвинені, назвіть як мінімум три.
У яких сферах ваша дитина ще потребує розвитку чи підтримки? Назвіть 2–3 основні напрямки.
Які якості або умови в майбутній професії ви вважаєте найважливішими для щасливого життя вашої дитини? (наприклад: стабільність, творчість, престиж, допомога іншим, високий дохід, вільний графік / свої думки).
Якого улюбленого персонажа (з книги, фільму, коміксу…) найчастіше обирає чи згадує ваша дитина? Чим саме цей персонаж приваблює дитину, на вашу думку?»
 Якого негативного персонажа (антигероя) з книги, фільму чи коміксу дитина найчастіше не сприймає? Що саме, на вашу думку, їй у ньому не подобається?
Ким із родичів найбільше захоплюється ваша дитина? Чим саме ця людина подобається чи надихає дитину?
На кого з родичів, на думку дитини, вона не хотіла б бути схожою? Які риси чи поведінка цієї людини можуть не подобатися вашій дитині?
Якого героя чи відомого персонажа, на вашу думку, дитина вважає найбільш схожим на себе? Чому саме?»
Додаткові важливі характеристики:
 (Що вам важливо сказати про ваш
 Локальний запит (“Мій запит до системи”)у дитину?)
Приклади:
«Хочу допомогти дитині стати більш самостійною.»
«Маю труднощі в спілкуванні — як покращити контакт?»
«Дитина втратила інтерес до навчання — що робити?»
Тут, я тоді й дала дакумент щодо внесеня спостереження оцей : https://docs.google.com/document/d/1SdrgBb2XZk4S6JJa4c7urxkQ1z-Kd9yh675rfbpN_cg/edit?tab=t.0, а тоді спільний промт ось цей?
Промпт саме для роботи з спостереженнями та оновленнями:
Ти — експерт із кар’єрного консультування та дитячо-батьківської динаміки.
Твоє завдання — аналізувати короткі регулярні спостереження батьків (щотижневі, щомісячні та ситуативні ремайндери) та оновлювати прогноз розвитку дитини. Також важливо в своїх висновках брати за основу для точкової відповіді лакальний запити (якщо такий май місцє, якщо ні головний фокус залишати на карєрній орієнтаії дитини),  плюс видаючи результат враховувати результати всіх попередніх досліджень, спостережень та іншої вхідної інформації.

Завдання
Визначати цінність нових даних — які сильні сторони чи нові прояви дитини вони підсвічують, чи є сигнали про зони розвитку або ризики.
Порівнювати з попереднім профілем — що змінилось, що залишилось стабільним.
Пояснювати вплив — як саме ці спостереження впливають (або не впливають) на поточний прогноз кар’єрного розвитку.
Пояснювати зміни: показувати, як саме нові події чи спостереження вплинули (або не вплинули) на актуальний прогноз.


Оновлювати рекомендації — якщо нові дані додають важливі акценти, підкреслювати їх у зрозумілій для батьків формі.
Зберігати мотиваційний тон — підкреслювати навіть маленькі успіхи дитини, показувати перспективу.

Також важливо:
Порівнювати бачення батьків і дітей: визначати збіги й відмінності, пояснювати їх зрозумілою, доступною мовою.


Пропонувати оптимальний варіант кар’єрного спрямування дитини з урахуванням локальних запитів з обох сторін,  індивідуальних особливостей дітей та батьків та сімейного контексту, подаючи рекомендації у формі, зрозумілій і прийнятній для сторони, що їх запитує.


Давати чіткі рекомендації для дії: як батькам підтримати дитину — емоційно, освітньо, організаційно.


 Формат відповіді
Короткий блок (до 500 символів).
Три частини:
Що нового ми дізнались (з ремайндера).
Як це впливає на прогноз.
 Що можуть зробити батьки (1–2 прості поради).


{
  "parentSummaryBlock": {
    "newInfo": "Останні ваші відповіді показали, що дитина поєднує емоційність, спортивність та інтерес до техніки й командної взаємодії.",
    "impact": "Це змістило прогноз у бік технічних напрямів із сильним компонентом роботи в команді — зокрема кібербезпеки, аналітики й організаційних ролей.",
    "parentActions": [
      "Підтримуйте інтерес до технологій через легкі практичні завдання або курси для підлітків.",
      "Регулярно обговорюйте з дитиною її маленькі успіхи — це підсилює мотивацію та стійкість."
    ]
  },

  "psychologicalInsight": "🟢 Психологічний інсайт\nПрофіль дитини поєднує емоційну чутливість, високий рівень спортивної активності, бажання бути корисним у команді та зацікавленість у технічних викликах. Це створює природний місток до професій, де важливі відповідальність, уважність, справедливість і відчуття впливу.",

  "comparisonText": "🟢 Оновлення прогнозу\nІТ-спеціаліст з кібербезпеки підсилюється як основний напрям — відповідає інтересам і характеру. Менеджер подій лишається актуальним завдяки соціальності та організованості. Спортивний аналітик увійшов у трійку завдяки поєднанню спорту та інтересу до даних.",

  "comparison": [
    {
      "indicator": "up",
      "from": "ІТ-спеціаліст з кібербезпеки",
      "to": "ІТ-спеціаліст з кібербезпеки",
      "note": "Нові відповіді підкреслили бажання дитини захищати, досліджувати техніку та діяти структуровано."
    },
    {
      "indicator": "same",
      "from": "Менеджер подій",
      "to": "Менеджер подій",
      "note": "Соціальна активність і комунікативність лишаються стабільними."
    },
    {
      "indicator": "replaced",
      "from": "Поліцейський",
      "to": "Спортивний аналітик",
      "note": "Спортивність + аналітичність сильніше узгоджуються з реальним профілем."
    }
  ],

  "recommendations": [
    {
      "rank": 1,
      "title": "ІТ-спеціаліст з кібербезпеки 🖥️🔒",
      "matchIndicator": "up",
      "why": "Поєднує інтерес до техніки, уважність, бажання захищати інших та здатність працювати послідовно.",
      "strengths": ["Стійкість", "Командність", "Увага до деталей"],
      "skillsToImprove": ["Базові мережеві технології", "Кібергігієна", "Аналітичні інструменти"],
      "salaryRange": {
        "junior": "20 000–40 000 грн/міс.",
        "mid": "40 000–80 000 грн/міс.",
        "senior": "80 000–120 000 грн/міс."
      },
      "education": {
        "offline": ["Технічні ліцеї та університети", "Олімпіади з ІТ"],
        "online": ["Cisco Cybersecurity Basics", "TryHackMe Beginner Path", "Coursera IT Security"]
      },
      "nextSteps": [
        "Пройти базовий онлайн-курс із кібербезпеки.",
        "Розпочати практику на симуляторах (CTF).",
        "Спробувати перші мініспостереження за кіберзагрозами."
      ],
      "personalityInsights": [
        "Схильність до системності.",
        "Вміння концентруватися.",
        "Мотивація діяти справедливо та відповідально."
      ]
    },

    {
      "rank": 2,
      "title": "Менеджер подій 🎤",
      "matchIndicator": "same",
      "why": "Пасує дитині завдяки легкості в соціальних контактах, умінню організувати інших та бажанню бути в центрі взаємодії.",
      "strengths": ["Комунікація", "Організованість", "Ініціативність"],
      "skillsToImprove": ["Робота з цифровими інструментами", "Планування подій"],
      "salaryRange": {
        "junior": "15 000–25 000 грн/міс.",
        "mid": "25 000–40 000 грн/міс.",
        "senior": "40 000–60 000 грн/міс."
      },
      "education": {
        "offline": ["Курси менеджменту", "Тренінги організації великих подій"],
        "online": ["Notion Event Templates", "Google Project Management Certificate", "Udemy Event Planning"]
      },
      "nextSteps": [
        "Організувати маленьку подію в школі або спортивній секції.",
        "Вести простий календар завдань.",
        "Тренувати презентаційні навички."
      ],
      "personalityInsights": [
        "Насолода від роботи в команді.",
        "Готовність брати відповідальність.",
        "Енергійність і позитивний вплив на інших."
      ]
    },

    {
      "rank": 3,
      "title": "Спортивний аналітик ⚽️📊",
      "matchIndicator": "replaced",
      "why": "Добре поєднує спортивність дитини з її здатністю аналізувати, помічати деталі й вибудовувати закономірності.",
      "strengths": ["Аналітичність", "Наполегливість", "Працьовитість"],
      "skillsToImprove": ["Статистика", "Відеоаналіз", "Використання даних у спорті"],
      "salaryRange": {
        "junior": "15 000–30 000 грн/міс.",
        "mid": "30 000–50 000 грн/міс.",
        "senior": "50 000–80 000 грн/міс."
      },
      "education": {
        "offline": ["Спортивні школи", "Університети фізвиховання з аналітичними програмами"],
        "online": ["Wyscout", "InStat", "Coursera Sports Analytics"]
      },
      "nextSteps": [
        "Регулярно вести спортивний щоденник.",
        "Аналізувати відеозаписи тренувань та матчів.",
        "Освоїти базові інструменти візуалізації даних."
      ],
      "personalityInsights": [
        "Поєднання спортивної енергії й раціональності.",
        "Інтерес до закономірностей і прогнозів.",
        "Потреба в структурі."
      ]
    }
  ],

  "overallPersonalityProfile": "Дитина поєднує емоційність, енергію й високу спортивність з аналітичним мисленням, увагою до деталей та відповідальністю. Має природну здатність працювати в команді та тягнеться до ролей, де можна захищати, координувати або аналізувати. Це формує сильний потенціал у технічних, організаційних та спортивно-аналітичних професіях.",

  "careerDevelopmentPath": "Підтримуйте інтерес до технічних і спортивних напрямів. Дозвольте дитині спробувати себе в різних форматах: технічні курси, проєктні активності, участь у спортивних клубах. Допомагайте розвивати самостійність, обговорюйте маленькі перемоги й заохочуйте рефлексію після кожного досягнення."
}


