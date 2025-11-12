import { useDispatch, useSelector } from 'react-redux'
import type { TypedUseSelectorHook } from 'react-redux'

import type { AppDispatch, RootState } from './index'

/**
 * Provides a typed dispatch hook scoped to the application store.
 *
 * @returns {AppDispatch} Redux dispatch function.
 */
export const useAppDispatch = () => useDispatch<AppDispatch>()
/**
 * Provides a typed selector hook scoped to the application store.
 */
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector

