import { Injectable } from '@angular/core';
import {
  CommiteeTaskEscalationDTO,
  SwaggerClient,
} from 'src/app/core/_services/swagger/SwaggerClient.service';

@Injectable({
  providedIn: 'root',
})
export class EscalationService {
  constructor(private swagger: SwaggerClient) {}

  addEscalation(escalation: CommiteeTaskEscalationDTO) {
    return this.swagger.apiCommiteeTaskEscalationInsertPost([escalation]);
  }

  editEscalation(escalation: CommiteeTaskEscalationDTO) {
    return this.swagger.apiCommiteeTaskEscalationUpdatePut([escalation]);
  }

  getEscalation(id: string) {
    return this.swagger.apiCommiteeTaskEscalationGetByIdGet(id);
  }
}
