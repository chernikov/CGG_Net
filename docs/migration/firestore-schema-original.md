# Firestore Database Schema

**Last Updated:** 2025-11-22
**Version:** 2.0 (Family-centered architecture)

---

## Overview

This document describes the complete Firestore database schema for the Career Guidance Guild application, including both existing collections (v1) and new family-centered collections (v2).

## Architecture Strategy

The schema uses a **hybrid approach** to maintain backward compatibility:
- **Legacy users** (v1): Continue to work with direct `users` collection
- **New users** (v2): Automatically create family groups with members
- **Migration path**: Existing users can optionally upgrade to family mode

---

## Collections

### 1. `users` (Core collection - v1 + v2)

Primary user accounts for authentication and profile data.

**Document ID:** Auto-generated or custom UID
**Indexes:** `email`, `emailVerificationToken`

```typescript
interface User {
  uid: string;
  email?: string;
  displayName?: string;
  dateOfBirth?: string; // ISO date string
  passwordHash?: string;
  createdAt: Timestamp;
  updatedAt: Timestamp;

  // Role and relationships
  role: 'user_child' | 'user_parent' | 'teacher';

  // Balance (current system uses credits)
  credits: number; // User balance in credits (1 credit = 1 token)

  // Email verification
  emailVerified?: boolean;
  verificationCode?: string;
  verificationCodeExpiry?: Timestamp;

  // Child-specific (legacy)
  parentId?: string; // Reference to parent user's uid
  schoolId?: string;

  // v2 Family references (NEW - optional)
  familyId?: string; // Reference to families/{id}
  memberId?: string; // Reference to members/{id}
}
```

**Sub-collections:**
- `transactions` - User transaction history (legacy, kept for backward compatibility)

---

### 2. `families` (NEW - v2)

Family groups that share Guild Points balance and manage multiple members.

**Document ID:** Auto-generated
**Indexes:** None initially

```typescript
interface Family {
  id: string; // Auto-generated
  name?: string; // Optional, default: "[Parent name]'s Family"
  credits: number; // Shared family balance in credits
  createdAt: Timestamp;
  updatedAt: Timestamp;
}
```

**Business Rules:**
- Created automatically when new user registers
- Balance is shared across all family members
- Parent role has full control over family resources

---

### 3. `members` (NEW - v2)

Junction table linking users to families with member-specific metadata.

**Document ID:** Auto-generated
**Indexes:** `userId`, `familyId`, `userId+familyId` (composite)

```typescript
interface Member {
  id: string; // Auto-generated
  userId: string; // Reference to users/{uid}
  familyId: string; // Reference to families/{id}
  nickname: string; // Display name within family
  avatar?: string; // Emoji or avatar URL
  isActive: boolean; // Soft delete flag
  createdAt: Timestamp;
  updatedAt: Timestamp;
}
```

**Business Rules:**
- One user can belong to only one family
- Member inherits role from linked User
- Deactivated members (isActive: false) don't appear in UI

---

### 4. `observations` (NEW - v2)

Weekly reflections and observations about family members.

**Document ID:** Auto-generated
**Indexes:** `memberId`, `familyId`, `processed`, `memberId+createdAt` (composite)

```typescript
interface Observation {
  id: string; // Auto-generated
  memberId: string; // Subject of observation (references members/{id})
  authorId: string; // User who wrote it (references users/{uid})
  familyId: string; // References families/{id}

  // Content
  category: 'mood' | 'relations' | 'achievements' | 'challenges' | 'interests' | 'other';
  title?: string; // Auto-generated or user-entered (max 40 chars)
  text: string; // Full observation text

  // AI processing
  processed: boolean; // Whether used in AI recommendation

  // Timestamps
  createdAt: Timestamp;
  updatedAt: Timestamp;
}
```

**Business Rules:**
- Family members can observe each other
- Observations marked as `processed: true` after AI analysis
- Used as input for AI recommendations

---

### 5. `recommendations` (NEW - v2)

AI-generated personalized recommendations based on observations and surveys.

**Document ID:** Auto-generated
**Indexes:** `memberId`, `familyId`, `memberId+createdAt` (composite)

```typescript
interface Recommendation {
  id: string; // Auto-generated
  memberId: string; // References members/{id}
  familyId: string; // References families/{id}

  // Content
  category: 'hard_skills' | 'soft_skills' | 'contact_interaction';
  title: string;
  text: string; // Full recommendation text
  insights: string[]; // Array of specific insights

  // Traceability
  sourceObservations: string[]; // Observation IDs used for this recommendation
  sourceSurveys?: string[]; // Survey IDs used (optional)
  aiRunNumber: number; // Incremental per member
  confidenceScore?: number; // 0-100 (optional)

  // Timestamps
  createdAt: Timestamp;
  analyzedAt: Timestamp; // When AI analysis was performed
}
```

**Business Rules:**
- Generated when family runs AI recommendation (costs 1 Guild Point)
- Requires at least 1 unprocessed observation
- Combines observations + latest survey results

---

### 6. `invitations` (NEW - v2)

Pending invitations to join family groups.

**Document ID:** Auto-generated
**Indexes:** `familyId`, `email`, `status`

```typescript
interface Invitation {
  id: string; // Auto-generated
  familyId: string; // References families/{id}
  email: string; // Invitee email

  // Invitee details
  role: 'parent' | 'child';
  nickname?: string; // Pre-filled if known
  dateOfBirth?: Timestamp; // Pre-filled if known

  // Status tracking
  status: 'pending' | 'accepted' | 'cancelled' | 'expired';
  invitedBy: string; // User ID who sent invitation (references users/{uid})

  // Timestamps
  sentAt: Timestamp;
  reminderSentAt?: Timestamp; // Last reminder email sent
  expiresAt: Timestamp; // Auto-expire after 30 days
  acceptedAt?: Timestamp;
}
```

**Business Rules:**
- Automatically expires after 30 days
- Reminder email sent after 7 days if still pending
- COPPA logic: If child <13, requires parental consent before sending

---

### 7. Survey Results Collections (Existing - v1)

Survey completion data for different survey types.

**Collections:**
- `classic-results`
- `gaming-results`
- `ab-test-results`

**Document ID:** Auto-generated survey ID
**Indexes:** `userId`, `updatedAt` (for ordering)

```typescript
interface SurveyResult {
  id: string; // Auto-generated

  // v1 fields (existing)
  userId?: string; // Legacy: references users/{uid}
  surveyType: 'classic' | 'gaming' | 'ab-test';
  language: string; // 'en' | 'uk' | 'hi'
  currentStep: number | 'feedback' | 'done';

  // Survey data
  steps?: StepData[]; // Step responses and AI results
  results?: AIResult[]; // Final profession matches

  // Timestamps
  createdAt: Timestamp;
  updatedAt: Timestamp;

  // v2 fields (NEW - optional)
  memberId?: string; // References members/{id}
  familyId?: string; // References families/{id}

  // Metadata
  metadata?: {
    schema?: string; // e.g., 'flat-v1'
    [key: string]: any;
  };
}
```

**Business Rules:**
- v1 users: linked via `userId` only
- v2 users: linked via `memberId` + `familyId` (userId still present)
- Results shown in Member Overview (Screen 7)

---

### 8. `transactions` (Existing - v1)

Payment and credit transactions.

**Document ID:** Order ID or auto-generated
**Indexes:** `userId`, `invoiceId`, `createdAt`

```typescript
interface Transaction {
  id: string; // Order ID or auto-generated
  userId: string; // References users/{uid}

  // Payment details
  amountKop: number; // Amount in kopiykas
  currency?: string; // 'UAH', 'USD', etc.
  provider?: 'monobank' | 'promo';

  // Monobank specific
  invoiceId?: string; // Monobank invoice ID
  pageUrl?: string; // Payment page URL

  // Status
  status: 'pending' | 'success' | 'failure' | 'expired';

  // Promo code
  promoCode?: string;
  promoExpiry?: Timestamp;

  // Timestamps
  createdAt: Timestamp;
  updatedAt?: Timestamp;
}
```

**Sub-collection location (legacy):**
- Also stored as `users/{uid}/transactions/{txId}` for some users

---

### 9. `system` (Configuration)

Global system settings and prompts.

**Document IDs:**
- `settings` - Global application settings
- `prompts` - AI prompt templates

#### `system/settings`
```typescript
interface SystemSettings {
  // Feature flags
  maintenanceMode?: boolean;
  registrationEnabled?: boolean;

  // Pricing
  defaultCredits?: number;

  // Limits
  maxSurveysPerUser?: number;

  // Updated timestamp
  updatedAt: Timestamp;
}
```

#### `system/prompts`
```typescript
interface SystemPrompts {
  // Survey analysis prompts
  classic_step1?: string;
  classic_step2?: string;
  gaming_step1?: string;
  // ... other prompts

  updatedAt: Timestamp;
}
```

---

### 10. `admin_users` (Admin management)

Administrator accounts with elevated permissions.

**Document ID:** Admin user's UID
**Indexes:** `role`

```typescript
interface AdminUser {
  uid: string;
  email: string;
  role: 'super_admin' | 'admin' | 'support';
  createdAt: Timestamp;
  createdBy?: string; // UID of admin who created this account
}
```

---

### 11. `impersonation_links` (Development/Support)

Temporary links for admin impersonation.

**Document ID:** Token hash
**Indexes:** `userId`, `expiresAt`

```typescript
interface ImpersonationLink {
  id: string; // Token hash
  userId: string; // Target user to impersonate
  createdBy: string; // Admin who created link
  expiresAt: Timestamp;
  createdAt: Timestamp;
}
```

---

### 12. `ai_logs` (Logging)

OpenAI API call logs for debugging and analytics.

**Document ID:** Auto-generated
**Indexes:** `userId`, `createdAt`

```typescript
interface AILog {
  id: string;
  userId?: string;
  surveyId?: string;
  step?: number;
  prompt?: string;
  response?: any;
  error?: string;
  tokensUsed?: number;
  createdAt: Timestamp;
}
```

---

## Firestore Rules

Production security rules implementation with role-based access control:

```javascript
rules_version = '2';
service cloud.firestore {
  match /databases/{database}/documents {

    // Helper function to check if user is a family member
    function isFamilyMember(familyId) {
      return request.auth != null &&
        exists(/databases/$(database)/documents/members/$(getMemberIdForUser())) &&
        get(/databases/$(database)/documents/members/$(getMemberIdForUser())).data.familyId == familyId;
    }

    // Helper function to get member ID for current user
    function getMemberIdForUser() {
      return get(/databases/$(database)/documents/users/$(request.auth.uid)).data.memberId;
    }

    // Helper function to check if user has parent role
    function isParent() {
      return request.auth != null &&
        get(/databases/$(database)/documents/users/$(request.auth.uid)).data.role == 'user_parent';
    }

    // Public survey collections (for anonymous surveys)
    match /classic-survey/{document} {
      allow read, write: if true;
    }
    match /classic-results/{document} {
      allow read, write: if true;
    }
    match /gaming-results/{document} {
      allow read, write: if true;
    }
    match /ab-test-results/{document} {
      allow read, write: if true;
    }
    match /ai_logs/{document} {
      allow read, write: if true;
    }
    match /debug-test/{document} {
      allow read, write: if true;
    }

    // Users - own data only
    match /users/{userId} {
      allow read, write: if request.auth != null && request.auth.uid == userId;
    }

    // Families - family members can read, parents can write
    match /families/{familyId} {
      allow read: if request.auth != null && isFamilyMember(familyId);
      allow write: if request.auth != null && isParent() && isFamilyMember(familyId);
    }

    // Members - authenticated users can read, users can write their own member data
    match /members/{memberId} {
      allow read: if request.auth != null;
      allow create: if request.auth != null;
      allow update, delete: if request.auth != null &&
        resource.data.userId == request.auth.uid;
    }

    // Observations - family members can read, authenticated users can create, authors can update/delete
    match /observations/{observationId} {
      allow read: if request.auth != null;
      allow create: if request.auth != null;
      allow update, delete: if request.auth != null &&
        resource.data.authorId == request.auth.uid;
    }

    // Recommendations - family members can read, write restricted to server/authenticated users
    match /recommendations/{recommendationId} {
      allow read: if request.auth != null;
      allow write: if request.auth != null;
    }

    // Invitations - authenticated users can read and manage invitations
    match /invitations/{invitationId} {
      allow read: if request.auth != null;
      allow create: if request.auth != null;
      allow update, delete: if request.auth != null &&
        resource.data.invitedBy == request.auth.uid;
    }

    // Transactions - users can read their own transactions
    match /transactions/{transactionId} {
      allow read: if request.auth != null &&
        resource.data.userId == request.auth.uid;
      allow write: if request.auth != null;
    }

    // All other collections require authentication
    match /{document=**} {
      allow read, write: if request.auth != null;
    }
  }
}
```

---

## Migration Path

### For New Users (Automatic)
1. User registers → Create `users` document
2. Create `families` document
3. Create `members` document linking user to family
4. Set `user.familyId` and `user.memberId`

### For Existing Users (Optional)
Run migration script to:
1. Create family for each existing user
2. Create member entry
3. Move `user.credits` → `family.credits`
4. Update `user.familyId` and `user.memberId`
5. Update survey results with `memberId` and `familyId`

See `.dev/scripts/migrate-to-families.ts` for implementation.

---

## Query Examples

Comprehensive examples of common Firestore queries for v2 collections.

### Family Queries

**Get family by ID:**
```typescript
const familyDoc = await db.collection('families').doc(familyId).get();
const family: Family = { id: familyDoc.id, ...familyDoc.data() };
```

**Get family for current user:**
```typescript
const userDoc = await db.collection('users').doc(userId).get();
const userData = userDoc.data();
if (userData?.familyId) {
  const familyDoc = await db.collection('families').doc(userData.familyId).get();
  const family: Family = { id: familyDoc.id, ...familyDoc.data() };
}
```

**Update family credits:**
```typescript
await db.collection('families').doc(familyId).update({
  credits: FieldValue.increment(-1), // Deduct 1 credit
  updatedAt: FieldValue.serverTimestamp()
});
```

### Member Queries

**Get all members of a family:**
```typescript
const snapshot = await db.collection('members')
  .where('familyId', '==', familyId)
  .where('isActive', '==', true)
  .get();
const members: Member[] = snapshot.docs.map(doc => ({
  id: doc.id,
  ...doc.data()
}));
```

**Get member by user ID:**
```typescript
const snapshot = await db.collection('members')
  .where('userId', '==', userId)
  .where('isActive', '==', true)
  .limit(1)
  .get();
const member = snapshot.docs[0];
```

**Get member details with user info:**
```typescript
const memberDoc = await db.collection('members').doc(memberId).get();
const memberData = memberDoc.data();
const userDoc = await db.collection('users').doc(memberData.userId).get();
const userData = userDoc.data();

const memberDetails = {
  ...memberData,
  email: userData.email,
  dateOfBirth: userData.dateOfBirth,
  role: userData.role
};
```

**Deactivate member (soft delete):**
```typescript
await db.collection('members').doc(memberId).update({
  isActive: false,
  updatedAt: FieldValue.serverTimestamp()
});
```

### Observation Queries

**Get observations for a member (paginated):**
```typescript
const snapshot = await db.collection('observations')
  .where('memberId', '==', memberId)
  .orderBy('createdAt', 'desc')
  .limit(20)
  .get();

const observations: Observation[] = snapshot.docs.map(doc => ({
  id: doc.id,
  ...doc.data()
}));
```

**Get unprocessed observations for a member:**
```typescript
const snapshot = await db.collection('observations')
  .where('memberId', '==', memberId)
  .where('processed', '==', false)
  .orderBy('createdAt', 'desc')
  .get();

const unprocessed: Observation[] = snapshot.docs.map(doc => ({
  id: doc.id,
  ...doc.data()
}));
```

**Get observations by category:**
```typescript
const snapshot = await db.collection('observations')
  .where('memberId', '==', memberId)
  .where('category', '==', 'achievements')
  .orderBy('createdAt', 'desc')
  .limit(10)
  .get();
```

**Mark observations as processed:**
```typescript
const batch = db.batch();
observationIds.forEach(id => {
  const ref = db.collection('observations').doc(id);
  batch.update(ref, {
    processed: true,
    updatedAt: FieldValue.serverTimestamp()
  });
});
await batch.commit();
```

**Get observation count by member:**
```typescript
const snapshot = await db.collection('observations')
  .where('memberId', '==', memberId)
  .get();
const count = snapshot.size;

// Count by category
const byCategory = {
  mood: 0,
  relations: 0,
  achievements: 0,
  challenges: 0,
  interests: 0,
  other: 0
};
snapshot.docs.forEach(doc => {
  const category = doc.data().category;
  byCategory[category]++;
});
```

### Recommendation Queries

**Get latest recommendations for a member:**
```typescript
const snapshot = await db.collection('recommendations')
  .where('memberId', '==', memberId)
  .orderBy('createdAt', 'desc')
  .limit(10)
  .get();

const recommendations: Recommendation[] = snapshot.docs.map(doc => ({
  id: doc.id,
  ...doc.data()
}));
```

**Get recommendations by category:**
```typescript
const snapshot = await db.collection('recommendations')
  .where('memberId', '==', memberId)
  .where('category', '==', 'hard_skills')
  .orderBy('createdAt', 'desc')
  .get();
```

**Get latest recommendation run number:**
```typescript
const snapshot = await db.collection('recommendations')
  .where('memberId', '==', memberId)
  .orderBy('aiRunNumber', 'desc')
  .limit(1)
  .get();

const latestRunNumber = snapshot.empty ? 0 : snapshot.docs[0].data().aiRunNumber;
const nextRunNumber = latestRunNumber + 1;
```

**Create new recommendations (batch):**
```typescript
const batch = db.batch();
recommendations.forEach(rec => {
  const ref = db.collection('recommendations').doc();
  batch.set(ref, {
    ...rec,
    createdAt: FieldValue.serverTimestamp(),
    analyzedAt: FieldValue.serverTimestamp()
  });
});
await batch.commit();
```

**Get recommendations with source observations:**
```typescript
const recDoc = await db.collection('recommendations').doc(recommendationId).get();
const recData = recDoc.data();

// Fetch source observations
const obsRefs = recData.sourceObservations.map(id =>
  db.collection('observations').doc(id)
);
const obsSnapshots = await db.getAll(...obsRefs);
const observations = obsSnapshots.map(doc => ({ id: doc.id, ...doc.data() }));
```

### Invitation Queries

**Get pending invitations for a family:**
```typescript
const snapshot = await db.collection('invitations')
  .where('familyId', '==', familyId)
  .where('status', '==', 'pending')
  .get();

const invitations: Invitation[] = snapshot.docs.map(doc => ({
  id: doc.id,
  ...doc.data()
}));
```

**Check if email already invited:**
```typescript
const snapshot = await db.collection('invitations')
  .where('email', '==', email)
  .where('status', 'in', ['pending', 'accepted'])
  .limit(1)
  .get();

const alreadyInvited = !snapshot.empty;
```

**Get expired invitations (for cleanup job):**
```typescript
const now = new Date();
const snapshot = await db.collection('invitations')
  .where('status', '==', 'pending')
  .where('expiresAt', '<', now)
  .get();

// Mark as expired
const batch = db.batch();
snapshot.docs.forEach(doc => {
  batch.update(doc.ref, { status: 'expired' });
});
await batch.commit();
```

**Accept invitation:**
```typescript
await db.collection('invitations').doc(invitationId).update({
  status: 'accepted',
  acceptedAt: FieldValue.serverTimestamp()
});
```

### Transaction Queries

**Get user's transaction history:**
```typescript
const snapshot = await db.collection('transactions')
  .where('userId', '==', userId)
  .orderBy('createdAt', 'desc')
  .limit(20)
  .get();

const transactions = snapshot.docs.map(doc => ({
  id: doc.id,
  ...doc.data()
}));
```

**Get successful transactions only:**
```typescript
const snapshot = await db.collection('transactions')
  .where('userId', '==', userId)
  .where('status', '==', 'success')
  .orderBy('createdAt', 'desc')
  .get();
```

**Find transaction by invoice ID:**
```typescript
const snapshot = await db.collection('transactions')
  .where('invoiceId', '==', invoiceId)
  .limit(1)
  .get();

const transaction = snapshot.empty ? null : {
  id: snapshot.docs[0].id,
  ...snapshot.docs[0].data()
};
```

### Survey Results Queries

**Get member's survey results:**
```typescript
const snapshot = await db.collection('classic-results')
  .where('memberId', '==', memberId)
  .orderBy('updatedAt', 'desc')
  .get();

const surveyResults = snapshot.docs.map(doc => ({
  id: doc.id,
  ...doc.data()
}));
```

**Get latest completed survey:**
```typescript
const snapshot = await db.collection('classic-results')
  .where('memberId', '==', memberId)
  .where('currentStep', '==', 'done')
  .orderBy('updatedAt', 'desc')
  .limit(1)
  .get();

const latestSurvey = snapshot.empty ? null : {
  id: snapshot.docs[0].id,
  ...snapshot.docs[0].data()
};
```

**Count completed surveys:**
```typescript
const snapshot = await db.collection('classic-results')
  .where('memberId', '==', memberId)
  .where('currentStep', '==', 'done')
  .get();

const completedCount = snapshot.size;
```

### Complex Queries (Multi-collection)

**Get family dashboard data (optimized):**
```typescript
// Single batch read for all family data
const familyRef = db.collection('families').doc(familyId);
const membersRef = db.collection('members').where('familyId', '==', familyId);
const invitationsRef = db.collection('invitations')
  .where('familyId', '==', familyId)
  .where('status', '==', 'pending');

const [familySnap, membersSnap, invitationsSnap] = await Promise.all([
  familyRef.get(),
  membersRef.get(),
  invitationsRef.get()
]);

const dashboardData = {
  family: { id: familySnap.id, ...familySnap.data() },
  members: membersSnap.docs.map(doc => ({ id: doc.id, ...doc.data() })),
  pendingInvitations: invitationsSnap.docs.map(doc => ({ id: doc.id, ...doc.data() }))
};
```

**Get member stats (for overview dashboard):**
```typescript
const [surveysSnap, observationsSnap, recommendationsSnap] = await Promise.all([
  db.collection('classic-results')
    .where('memberId', '==', memberId)
    .where('currentStep', '==', 'done')
    .get(),
  db.collection('observations')
    .where('memberId', '==', memberId)
    .get(),
  db.collection('recommendations')
    .where('memberId', '==', memberId)
    .get()
]);

const stats = {
  testsPassed: surveysSnap.size,
  observationsTotal: observationsSnap.size,
  observationsByCategory: {
    mood: 0,
    relations: 0,
    achievements: 0,
    challenges: 0,
    interests: 0,
    other: 0
  },
  recommendationsTotal: recommendationsSnap.size,
  lastUpdate: null as Date | null
};

// Count observations by category
observationsSnap.docs.forEach(doc => {
  const category = doc.data().category;
  stats.observationsByCategory[category]++;
});

// Get last update timestamp
const allDocs = [
  ...surveysSnap.docs,
  ...observationsSnap.docs,
  ...recommendationsSnap.docs
];
if (allDocs.length > 0) {
  const timestamps = allDocs.map(doc => doc.data().updatedAt?.toDate() || doc.data().createdAt?.toDate());
  stats.lastUpdate = new Date(Math.max(...timestamps.map(d => d.getTime())));
}
```

---

## API Endpoints

### New endpoints required for v2:

```
GET    /api/family                       → Get current user's family
POST   /api/family                       → Create family
PATCH  /api/family                       → Update family

GET    /api/family/members               → List family members
POST   /api/family/members               → Add member / send invitation
GET    /api/family/members/:id           → Get member details
PATCH  /api/family/members/:id           → Update member
DELETE /api/family/members/:id           → Delete member

GET    /api/family/invitations           → List pending invitations
POST   /api/family/invitations/:id/resend → Resend invitation
DELETE /api/family/invitations/:id       → Cancel invitation
POST   /api/invitations/:token/accept    → Accept invitation (public)

GET    /api/observations?memberId={id}   → List observations
POST   /api/observations                 → Create observation
GET    /api/observations/:id             → Get observation
PATCH  /api/observations/:id             → Update observation
DELETE /api/observations/:id             → Delete observation

GET    /api/recommendations?memberId={id} → List recommendations
POST   /api/ai/recommendations           → Generate recommendations (costs 1 GP)
GET    /api/recommendations/:id          → Get recommendation
```

---

## Performance Considerations

### Indexes to Create

Firestore requires composite indexes for queries with multiple filters or ordering. Create these indexes via Firebase Console or using the following index definitions:

#### Single-field Indexes (Auto-created)
```
users: email (asc)
users: emailVerificationToken (asc)
users: familyId (asc)
users: memberId (asc)
members: userId (asc)
members: familyId (asc)
observations: memberId (asc)
observations: familyId (asc)
observations: processed (asc)
observations: authorId (asc)
recommendations: memberId (asc)
recommendations: familyId (asc)
invitations: familyId (asc)
invitations: email (asc)
invitations: status (asc)
transactions: userId (asc)
transactions: invoiceId (asc)
```

#### Composite Indexes (Must be created manually)

**Members Collection:**
```json
{
  "collectionGroup": "members",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "userId", "order": "ASCENDING" },
    { "fieldPath": "familyId", "order": "ASCENDING" }
  ]
}
```

**Observations Collection:**
```json
{
  "collectionGroup": "observations",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "memberId", "order": "ASCENDING" },
    { "fieldPath": "createdAt", "order": "DESCENDING" }
  ]
},
{
  "collectionGroup": "observations",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "familyId", "order": "ASCENDING" },
    { "fieldPath": "processed", "order": "ASCENDING" }
  ]
},
{
  "collectionGroup": "observations",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "memberId", "order": "ASCENDING" },
    { "fieldPath": "processed", "order": "ASCENDING" },
    { "fieldPath": "createdAt", "order": "DESCENDING" }
  ]
}
```

**Recommendations Collection:**
```json
{
  "collectionGroup": "recommendations",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "memberId", "order": "ASCENDING" },
    { "fieldPath": "createdAt", "order": "DESCENDING" }
  ]
},
{
  "collectionGroup": "recommendations",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "familyId", "order": "ASCENDING" },
    { "fieldPath": "createdAt", "order": "DESCENDING" }
  ]
},
{
  "collectionGroup": "recommendations",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "memberId", "order": "ASCENDING" },
    { "fieldPath": "category", "order": "ASCENDING" },
    { "fieldPath": "createdAt", "order": "DESCENDING" }
  ]
}
```

**Invitations Collection:**
```json
{
  "collectionGroup": "invitations",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "familyId", "order": "ASCENDING" },
    { "fieldPath": "status", "order": "ASCENDING" }
  ]
},
{
  "collectionGroup": "invitations",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "email", "order": "ASCENDING" },
    { "fieldPath": "status", "order": "ASCENDING" }
  ]
},
{
  "collectionGroup": "invitations",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "status", "order": "ASCENDING" },
    { "fieldPath": "expiresAt", "order": "ASCENDING" }
  ]
}
```

**Transactions Collection:**
```json
{
  "collectionGroup": "transactions",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "userId", "order": "ASCENDING" },
    { "fieldPath": "createdAt", "order": "DESCENDING" }
  ]
},
{
  "collectionGroup": "transactions",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "userId", "order": "ASCENDING" },
    { "fieldPath": "status", "order": "ASCENDING" },
    { "fieldPath": "createdAt", "order": "DESCENDING" }
  ]
}
```

**Survey Results Collections:**
```json
{
  "collectionGroup": "classic-results",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "userId", "order": "ASCENDING" },
    { "fieldPath": "updatedAt", "order": "DESCENDING" }
  ]
},
{
  "collectionGroup": "classic-results",
  "queryScope": "COLLECTION",
  "fields": [
    { "fieldPath": "memberId", "order": "ASCENDING" },
    { "fieldPath": "updatedAt", "order": "DESCENDING" }
  ]
}
```

### Query Optimization Best Practices

1. **Pagination**: Always use `limit()` with cursor-based pagination
   ```typescript
   const query = db.collection('observations')
     .where('memberId', '==', memberId)
     .orderBy('createdAt', 'desc')
     .limit(20)
     .startAfter(lastDoc);
   ```

2. **Denormalization**: Store frequently accessed data redundantly
   - Store member nickname in observations for faster display
   - Cache family credits in localStorage with TTL

3. **Batch Reads**: Use `getAll()` for multiple documents
   ```typescript
   const refs = memberIds.map(id => db.collection('members').doc(id));
   const snapshots = await db.getAll(...refs);
   ```

4. **Query Limits**: Never fetch unbounded collections
   - Default limit: 20 items per page
   - Max limit: 100 items per page

5. **Caching Strategy**:
   - Client-side cache for family balance (TTL: 5 minutes)
   - Cache member list (TTL: 10 minutes)
   - Cache survey results (TTL: 1 hour)

---

## Collection Relationships & Data Flow

### Entity Relationship Diagram (ERD)

```
┌─────────────────┐
│     users       │
│  (uid: string)  │
└────────┬────────┘
         │
         │ 1:1
         ├──────────────────────────────┐
         │                              │
         ▼                              ▼
┌─────────────────┐           ┌─────────────────┐
│    members      │  N:1      │    families     │
│  (id: string)   │◄──────────┤  (id: string)   │
└────────┬────────┘           └────────┬────────┘
         │                              │
         │ 1:N                          │ 1:N
         ├──────────────────────────────┤
         │                              │
         ▼                              ▼
┌─────────────────┐           ┌─────────────────┐
│  observations   │           │   invitations   │
│  (id: string)   │           │  (id: string)   │
└────────┬────────┘           └─────────────────┘
         │
         │ N:N (via sourceObservations[])
         │
         ▼
┌─────────────────┐
│ recommendations │
│  (id: string)   │
└────────┬────────┘
         │
         │ 1:N (optional)
         ▼
┌─────────────────┐
│ survey-results  │
│  (id: string)   │
└─────────────────┘
```

### Relationship Details

#### 1. **User ↔ Member ↔ Family** (1:1:1)

**Flow:**
```
User Registration → Create User → Create Family → Create Member → Link All Three
```

**Fields:**
- `users.familyId` → references `families.id`
- `users.memberId` → references `members.id`
- `members.userId` → references `users.uid`
- `members.familyId` → references `families.id`

**Business Rules:**
- Each user belongs to exactly ONE family
- Each user has exactly ONE member profile
- Each member belongs to exactly ONE family
- Families can have MULTIPLE members

**Example:**
```typescript
// User document
{
  uid: "user_abc123",
  email: "parent@example.com",
  role: "user_parent",
  familyId: "family_xyz789",  // → families/family_xyz789
  memberId: "member_def456"   // → members/member_def456
}

// Member document
{
  id: "member_def456",
  userId: "user_abc123",      // → users/user_abc123
  familyId: "family_xyz789",  // → families/family_xyz789
  nickname: "Mom",
  isActive: true
}

// Family document
{
  id: "family_xyz789",
  name: "Smith Family",
  credits: 100
}
```

#### 2. **Family → Members** (1:N)

**Flow:**
```
Family ─┬→ Member 1 (Parent)
        ├→ Member 2 (Child 1)
        └→ Member 3 (Child 2)
```

**Query Pattern:**
```typescript
// Get all members of a family
const members = await db.collection('members')
  .where('familyId', '==', familyId)
  .where('isActive', '==', true)
  .get();
```

**Business Rules:**
- Family can have 1-10 members (configurable limit)
- At least ONE parent role required per family
- Soft delete: set `member.isActive = false` instead of deleting

#### 3. **Member → Observations** (1:N)

**Flow:**
```
Member ─┬→ Observation 1 (mood)
        ├→ Observation 2 (achievements)
        └→ Observation 3 (challenges)
```

**Fields:**
- `observations.memberId` → references `members.id` (subject)
- `observations.authorId` → references `users.uid` (author)
- `observations.familyId` → references `families.id` (for access control)

**Business Rules:**
- Any family member can observe any other family member
- Author can edit/delete their own observations
- Observations can be about oneself (memberId == authorId's member)

**Example:**
```typescript
// Parent observing child
{
  id: "obs_001",
  memberId: "member_child_1",   // Child being observed
  authorId: "user_parent_1",     // Parent who wrote it
  familyId: "family_xyz789",
  category: "achievements",
  text: "Won the school math competition!",
  processed: false
}
```

#### 4. **Member → Recommendations** (1:N)

**Flow:**
```
Member ─┬→ Recommendation 1 (hard_skills)
        ├→ Recommendation 2 (soft_skills)
        └→ Recommendation 3 (contact_interaction)
```

**Fields:**
- `recommendations.memberId` → references `members.id`
- `recommendations.familyId` → references `families.id`
- `recommendations.sourceObservations[]` → array of `observations.id`
- `recommendations.sourceSurveys[]` → array of survey result IDs

**Business Rules:**
- Created by AI engine (not directly by users)
- Requires ≥1 unprocessed observation OR new survey results
- Costs 1 Guild Point from family balance
- `aiRunNumber` increments per member (not global)

**Example:**
```typescript
{
  id: "rec_001",
  memberId: "member_child_1",
  familyId: "family_xyz789",
  category: "hard_skills",
  title: "Develop Python Programming Skills",
  text: "Based on math competition success...",
  insights: ["Strong analytical thinking", "Problem-solving aptitude"],
  sourceObservations: ["obs_001", "obs_002", "obs_003"],
  sourceSurveys: ["survey_classic_123"],
  aiRunNumber: 3,  // 3rd AI run for this member
  confidenceScore: 85
}
```

#### 5. **Observations ↔ Recommendations** (N:N)

**Flow:**
```
Observations ─┬→ obs_001 ──┐
              ├→ obs_002 ──┼→ Recommendation 1
              └→ obs_003 ──┘

              ┌→ obs_004 ──┐
              ├→ obs_005 ──┼→ Recommendation 2
              └→ obs_006 ──┘
```

**Relationship:**
- One recommendation uses MULTIPLE observations
- One observation can be used in MULTIPLE recommendations (across different AI runs)
- Tracked via `recommendations.sourceObservations[]` array

**Processing Flow:**
1. User creates observations → `processed: false`
2. AI engine runs → fetches unprocessed observations
3. AI generates recommendations → stores observation IDs
4. Mark observations as `processed: true`
5. Observations remain in DB (audit trail)

#### 6. **Family → Invitations** (1:N)

**Flow:**
```
Family ─┬→ Invitation 1 (pending)
        ├→ Invitation 2 (accepted)
        └→ Invitation 3 (expired)
```

**Fields:**
- `invitations.familyId` → references `families.id`
- `invitations.invitedBy` → references `users.uid`

**Lifecycle:**
```
pending → accepted  → (create member & user)
        ↘ cancelled → (do nothing)
        ↘ expired   → (auto-expire after 30 days)
```

**Business Rules:**
- Only parents can send invitations
- Email uniqueness check before sending
- Auto-expire after 30 days
- Send reminder after 7 days if still pending

#### 7. **Member → Survey Results** (1:N, Optional)

**Flow:**
```
Member ─┬→ classic-results/survey_1
        ├→ classic-results/survey_2
        └→ gaming-results/survey_3
```

**Fields:**
- `survey-results.memberId` → references `members.id` (NEW in v2)
- `survey-results.familyId` → references `families.id` (NEW in v2)
- `survey-results.userId` → references `users.uid` (LEGACY, still present)

**Backward Compatibility:**
- v1 users: only `userId` set
- v2 users: `userId`, `memberId`, and `familyId` all set
- Queries support both patterns

#### 8. **User → Transactions** (1:N)

**Flow:**
```
User ─┬→ Transaction 1 (purchase 100 credits)
      ├→ Transaction 2 (purchase 500 credits)
      └→ Transaction 3 (promo code redemption)
```

**Fields:**
- `transactions.userId` → references `users.uid`

**Business Rules:**
- Transactions update user's family balance (via `users.familyId`)
- Webhook from Monobank triggers balance update
- Promo codes stored in transaction record

### Data Flow Diagrams

#### Registration Flow (v2)
```
1. User registers
   ↓
2. Create users/{uid}
   ↓
3. Create families/{familyId} (auto-generated)
   ↓
4. Create members/{memberId}
   ↓
5. Update users/{uid} with familyId & memberId
   ↓
6. Send email verification
```

#### Add Member Flow
```
1. Parent clicks "Add Member"
   ↓
2. Create invitations/{id} with status: 'pending'
   ↓
3. Send invitation email
   ↓
4. Invitee clicks link → Accept
   ↓
5. If email exists: link to existing user
   If new email: create new user
   ↓
6. Create members/{id} for invitee
   ↓
7. Update invitations/{id} status: 'accepted'
```

#### AI Recommendation Flow
```
1. User clicks "Run AI Recommendation"
   ↓
2. Check family.credits >= 1
   ↓
3. Fetch unprocessed observations (where processed: false)
   ↓
4. Fetch latest survey results (optional)
   ↓
5. Call OpenAI API with combined context
   ↓
6. Parse AI response → create recommendations/{id} (batch)
   ↓
7. Mark observations as processed: true (batch)
   ↓
8. Deduct 1 credit: families/{id}.credits -= 1
   ↓
9. Return recommendations to client
```

#### Payment Flow
```
1. User selects pricing plan
   ↓
2. Create transactions/{id} with status: 'pending'
   ↓
3. Call Monobank API → get payment URL
   ↓
4. User completes payment
   ↓
5. Monobank webhook → POST /api/payment/monobank
   ↓
6. Verify signature
   ↓
7. Update transactions/{id} status: 'success'
   ↓
8. Add credits to families/{familyId}.credits
```

### Referential Integrity

**No foreign key constraints** in Firestore (NoSQL), so implement application-level integrity:

#### On Delete Cascades

**Delete User:**
```typescript
// Must handle manually
1. Deactivate member: members/{id}.isActive = false
2. Do NOT delete observations (keep for audit)
3. Do NOT delete recommendations (keep for audit)
4. Transfer family ownership if user is last parent
```

**Delete Family (not supported):**
- Families cannot be deleted, only deactivated
- Alternative: mark all members as inactive

**Delete Member:**
- Soft delete: set `isActive: false`
- Observations remain (historical record)
- Recommendations remain (historical record)

#### Orphan Prevention

**Before creating observation:**
```typescript
// Verify member exists and belongs to user's family
const member = await db.collection('members').doc(memberId).get();
if (!member.exists || member.data().familyId !== userFamilyId) {
  throw new Error('Invalid member');
}
```

**Before deducting credits:**
```typescript
// Verify family has sufficient balance
const family = await db.collection('families').doc(familyId).get();
if (family.data().credits < 1) {
  throw new Error('Insufficient credits');
}
```

### Performance Optimization

**Denormalization Strategy:**

Store redundant data to avoid joins:

```typescript
// ✅ GOOD: Denormalize member nickname in observation
{
  id: "obs_001",
  memberId: "member_child_1",
  memberNickname: "Alice",  // ← Denormalized for fast display
  authorId: "user_parent_1",
  authorName: "Mom",         // ← Denormalized
  text: "..."
}

// ❌ BAD: Require join to get member name
{
  id: "obs_001",
  memberId: "member_child_1",  // Must fetch members/{id} to get name
  text: "..."
}
```

**When to denormalize:**
- Display names (frequently shown, rarely change)
- Counters (observation counts, recommendation counts)
- Timestamp of last activity

**When NOT to denormalize:**
- Credits balance (changes frequently, must be atomic)
- User email (privacy concerns)
- Large nested objects

---

## Security Notes

1. **PII Protection**: Observations may contain sensitive data
2. **Access Control**: Implement row-level security in Firestore rules
3. **Rate Limiting**: Limit AI recommendation calls to prevent abuse
4. **COPPA Compliance**: Validate child age before allowing registration
5. **Email Privacy**: Hash or encrypt email in invitations

---

## Questions & Decisions

- [ ] Should `members` have a separate `role` field or inherit from `users.role`?
- [ ] Max observations per member before requiring cleanup?
- [ ] Retention policy for old recommendations?
- [ ] Support multiple families per user in future?
- [ ] Archive vs hard delete for deactivated members?

---

## References

- [Firestore Best Practices](https://firebase.google.com/docs/firestore/best-practices)
- [Implementation Plan](./requirements/IMPLEMENTATION_PLAN.md)
- [Token Economy](./token-economy.md)
