/**
 * Create a jest spy on a function marked as "private", bypassing the type system temporarily.
 * Pass generic params to match the type signature of the function, if you wish.
 *
 * @param spyObject The object that owns the function
 * @param functionName Name of the function to spy on
 */
export default function spyOnPrivateFunction<TReturnType extends any = any, TParams extends any[] = any[]>(spyObject: unknown, functionName: string): jest.SpyInstance<TReturnType, TParams> {
  return jest.spyOn(spyObject as any, functionName) as unknown as jest.SpyInstance<TReturnType, TParams>;
}
