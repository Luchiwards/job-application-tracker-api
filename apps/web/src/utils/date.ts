const createParsedDate = (value: string): Date | null => {
  const parsed = new Date(value)
  if (Number.isNaN(parsed.valueOf())) {
    return null
  }

  return parsed
}

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


