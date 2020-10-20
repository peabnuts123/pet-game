type DeepPartial<T> = {
  [P in keyof T]?: T[P] | DeepPartial<T[P]>;
};
export default DeepPartial;
