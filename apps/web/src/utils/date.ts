/**
 * Attempts to parse a date string and returns null when invalid.
 *
 * @param {string} value Date string to parse.
 * @returns {Date | null} Parsed `Date` or null if invalid.
 */
const createParsedDate = (value: string): Date | null => {
  const parsed = new Date(value)
  if (Number.isNaN(parsed.valueOf())) {
    return null
  }

  return parsed
}

/**
 * Formats a date string into a localized short date (e.g., Jan 1, 2024).
 *
 * @param {string} value Date string to format.
 * @returns {string} Localized short date or original value if invalid.
 */
export const formatDate = (value: string): string => {
  const parsed = createParsedDate(value)
  if (!parsed) {
    return value
  }

  return parsed.toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })
}

/**
 * Formats a date string into a localized date with time component.
 *
 * @param {string} value Date string to format.
 * @returns {string} Localized date/time or original value if invalid.
 */
export const formatDateTime = (value: string): string => {
  const parsed = createParsedDate(value)
  if (!parsed) {
    return value
  }

  return parsed.toLocaleString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}


