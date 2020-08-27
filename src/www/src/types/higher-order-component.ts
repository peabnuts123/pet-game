import { ComponentType, h } from "preact";

type HigherOrderComponent = <TProps extends Record<string, unknown>>(Component: ComponentType<TProps>) => (props: TProps) => h.JSX.Element;

export default HigherOrderComponent;
