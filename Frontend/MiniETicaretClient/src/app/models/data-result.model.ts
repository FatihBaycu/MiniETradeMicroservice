import { Result } from "./result.model";

export interface DataResult<T> extends Result {
    data: T;
}