import { Component, OnInit,Input } from '@angular/core';
import { MeetingCommentDTO, SurveyAnswerUserDTO, UserDetailsDTO } from 'src/app/core/_services/swagger/SwaggerClient.service';

@Component({
  selector: 'app-print-recommendations',
  templateUrl: './print-recommendations.component.html',
  styleUrls: ['./print-recommendations.component.scss']
})
export class PrintRecommendationsComponent implements OnInit {
  @Input('recommendations') recommendations:MeetingCommentDTO[];
  allUsers:any[] = []
  constructor() { }

  ngOnInit(): void {
    setTimeout(()=>{
      this.recommendations.map((res) => {
        res.surveyAnswers.map((ans) => {
          ans.surveyAnswerUsers.map((user) => {
            this.allUsers.push({
              id:res.id,
              surveyAnswerId:user.surveyAnswerId,
              answer:ans.answer,
              user:new UserDetailsDTO({
                fullNameAr:user.user.fullNameAr,
                fullNameEn:user.user.fullNameEn
              })
            })
          })
        })
      })
    },500)
  }

}
