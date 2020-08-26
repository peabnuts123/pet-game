import { ComponentType, h } from "preact";

type HigherOrderComponent = <TProps extends {}>(Component: ComponentType<TProps>) => (props: TProps) => h.JSX.Element;

export default HigherOrderComponent;
