import type { ChangeEvent } from 'react'

type PaginationControlsProps = {
  page: number
  pageSize: number
  totalPages: number
  totalCount: number
  onPageChange: (page: number) => void
  onPageSizeChange?: (pageSize: number) => void
  pageSizeOptions?: number[]
  isLoading?: boolean
}

const PaginationControls = ({
  page,
  pageSize,
  totalPages,
  totalCount,
  onPageChange,
  onPageSizeChange,
  pageSizeOptions = [10, 20, 50],
  isLoading = false,
}: PaginationControlsProps) => {
  const handlePrevious = () => {
    if (page > 1) {
      onPageChange(page - 1)
    }
  }

  const handleNext = () => {
    if (page < totalPages) {
      onPageChange(page + 1)
    }
  }

  const handlePageSizeChange = (event: ChangeEvent<HTMLSelectElement>) => {
    const nextPageSize = Number.parseInt(event.target.value, 10)
    onPageSizeChange?.(nextPageSize)
  }

  const disableNavigation = isLoading || totalPages === 0

  return (
    <div className="pagination">
      <button
        type="button"
        className="pagination__button"
        onClick={handlePrevious}
        disabled={disableNavigation || page <= 1}
      >
        Previous
      </button>
      <span className="pagination__status">
        Page {totalPages > 0 ? page : 0} of {totalPages}
      </span>
      <button
        type="button"
        className="pagination__button"
        onClick={handleNext}
        disabled={disableNavigation || page >= totalPages}
      >
        Next
      </button>
      <div className="pagination__details">
        <span>{totalCount} total</span>
        {onPageSizeChange ? (
          <label className="pagination__page-size">
            Show
            <select value={pageSize} onChange={handlePageSizeChange} disabled={isLoading}>
              {pageSizeOptions.map((option) => (
                <option key={option} value={option}>
                  {option}
                </option>
              ))}
            </select>
            per page
          </label>
        ) : null}
      </div>
    </div>
  )
}

export default PaginationControls

