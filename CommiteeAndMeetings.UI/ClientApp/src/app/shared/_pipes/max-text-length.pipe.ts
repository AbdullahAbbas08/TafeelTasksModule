import { PipeTransform, Pipe } from "@angular/core";

@Pipe({
  name: "maxLenth"
})
export class MaxLengthPipe implements PipeTransform {
  transform(value: any, limit: number) {
    if (value && value.length > limit) {
      return value.substr(0, limit) + " ...";
    }
    return value;
  }
}
