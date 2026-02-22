/** Maps backend error messages to i18n translation keys */
export const backendErrorKeys: Record<string, string> = {
  'Invalid email or password':              'errors.invalidCredentials',
  'Invalid credentials':                    'errors.invalidCredentials',
  'User with this email already exists':    'errors.userAlreadyExists',
};

export function toTranslationKey(message: string | undefined | null): string {
  if (!message) return 'errors.unknown';
  return backendErrorKeys[message] ?? 'errors.unknown';
}
