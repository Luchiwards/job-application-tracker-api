import type { ReactNode } from 'react'

type AlertVariant = 'success' | 'error' | 'info'

type AlertProps = {
  variant?: AlertVariant
  children: ReactNode
  onClose?: () => void
  dismissible?: boolean
}

const variantClassName: Record<AlertVariant, string> = {
  success: 'alert--success',
  error: 'alert--error',
  info: 'alert--info',
}

/**
 * Shows contextual feedback with optional dismissal capability.
 *
 * @param {AlertProps} props Component props including variant, content, and dismissal handler.
 * @returns {JSX.Element} Styled alert banner element.
 */
const Alert = ({ variant = 'info', children, onClose, dismissible = false }: AlertProps) => (
  <div className={`alert ${variantClassName[variant]}`}>
    <div className="alert__content">{children}</div>
    {dismissible ? (
      <button type="button" className="alert__dismiss" onClick={onClose} aria-label="Dismiss">
        ×
      </button>
    ) : null}
  </div>
)

export default Alert

