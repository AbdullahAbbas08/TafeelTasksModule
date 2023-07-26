import { animate, state, style, transition, trigger } from '@angular/animations';

export const BackgroundTrigger = trigger('AddEditTrigger', [
  state(
    'edit',
    style({
      backgroundColor: 'lightGreen',
    })
  ),
  state(
    'add',
    style({
      backgroundColor: '#f5d166',
    })
  ),
  transition("add <=> edit", animate('300ms'))
]);
