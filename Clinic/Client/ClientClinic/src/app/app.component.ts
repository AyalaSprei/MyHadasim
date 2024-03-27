import { Component, OnInit } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Member, MemberMinimal, Immune } from './models';
import { DatePipe } from '@angular/common';
import { ChartData } from 'chart.js';
import { FormControl, FormGroup } from '@angular/forms';
import { FileUploader } from 'ng2-file-upload';
import { Observable } from 'rxjs';
import { MessageService } from 'primeng/api';

interface UploadEvent {
  originalEvent: any;
  files: File[];
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [MessageService]

})
export class AppComponent implements OnInit {

  constructor(
    private http: HttpClient,
    private messageService: MessageService
  ) {
    this.fetchData();
  } 

  title = 'CoronaClient';
  members: MemberMinimal[] = [];
  selectedMember: MemberMinimal | undefined;
  fullMember: Member | undefined;
  visible: boolean = false;
  immunes: Immune[] = [
    { date: new Date(), creatorId: 0 },
    { date: new Date(), creatorId: 0 },
    { date: new Date(), creatorId: 0 },
    { date: new Date(), creatorId: 0 },
  ];

  fetchData() {
    this.http.get<MemberMinimal[]>('/api/MinimalApi').subscribe((data) => {
      this.members = data;
      console.log(data);
    });
  }

  showDialog(member: MemberMinimal) {
    this.selectedMember = member;
    this.getFullMember(this.selectedMember.id);
    this.visible = true;
  }
  getFullMember(id: number) {
    this.http.get<Member>(`/api/MinimalApi/${id}`).subscribe((data) => {
      this.fullMember = data;
      this.fullMember.immunes?.forEach((detail, i) => {
        this.immunes[i].date = detail.date;
        this.immunes[i].creatorId = detail.creatorId;
      });
      console.log(data);
    });
  }

//#region form section

  visibleForm: boolean = false;

  immunesCount = 1;

  creators: { creatorId: number; creatorName: string }[] = [
    { creatorId: 1, creatorName: 'Faizer' },
    { creatorId: 2, creatorName: 'Moderna' },
  ];

  FormTitle: string = '';

  newMember: Member = {
    name: '',
    lastName: '',
    identityNumber: '',
    city: '',
    street: '',
    homeNumber: '',
    birthdate: new Date(),
    telephone: '',
    mobilePhone: '',
    isImmune: false,
    immunes: undefined,
    positiveDate: undefined,
    negativeDate: undefined,
    profilePicture: '',
  };

  openCreateMemberForm(title: string) {
    this.newMember.name = '';
    this.newMember.lastName = '';
    this.newMember.identityNumber = '';
    this.newMember.city = '';
    this.newMember.street = '';
    this.newMember.homeNumber = '';
    this.newMember.birthdate = undefined;
    this.newMember.telephone = '';
    this.newMember.mobilePhone = '';
    this.newMember.isImmune = false;
    this.newMember.positiveDate = undefined;
    this.newMember.negativeDate = undefined;
    this.newMember.profilePicture = '';
    this.immunesCount = 1;
    this.immunes = [
      { date: new Date(), creatorId: 0 },
      { date: new Date(), creatorId: 0 },
      { date: new Date(), creatorId: 0 },
      { date: new Date(), creatorId: 0 },
    ];
    this.FormTitle = title;
    this.visibleForm = true;
    console.log(this.newMember);
  }

  openUpdateMemberForm(title: string) {
    if (this.fullMember?.isImmune) {
      this.immunesCount = 0;
      this.fullMember.immunes?.forEach((immune) => {
        if (immune.creatorId !== 0) {
          this.immunesCount++;
        }
      });
    }
    this.FormTitle = title;
    this.visible = false;
    this.visibleForm = true;

    if (this.fullMember) {
      this.newMember.name = this.fullMember?.name || '';
      this.newMember.lastName = this.fullMember?.lastName || '';
      this.newMember.identityNumber = this.fullMember?.identityNumber || '';
      this.newMember.city = this.fullMember?.city || '';
      this.newMember.street = this.fullMember?.street || '';
      this.newMember.homeNumber = this.fullMember?.homeNumber || '';
      this.newMember.birthdate = this.fullMember?.birthdate;
      this.newMember.telephone = this.fullMember?.telephone || '';
      this.newMember.mobilePhone = this.fullMember?.mobilePhone || '';
      this.newMember.isImmune = this.fullMember?.isImmune || false;
      this.newMember.positiveDate = this.fullMember?.positiveDate || undefined;
      this.newMember.negativeDate = this.fullMember?.negativeDate || undefined;
      this.newMember.profilePicture = this.fullMember?.profilePicture || '';
    }
  }
  addImune() {
    console.log(this.immunesCount);
    this.immunesCount++;
  }
  Save() {
    this.newMember.immunes = this.immunes;
    if (this.FormTitle == 'update') {
      this.update();
    } else {
      this.createMember();
    }
    this.immunesCount = 1;
    this.immunes = [
      { date: new Date(), creatorId: 0 },
      { date: new Date(), creatorId: 0 },
      { date: new Date(), creatorId: 0 },
      { date: new Date(), creatorId: 0 },
    ];
  }
  update() {
    console.log(this.newMember);
    const url = `/api/MinimalApi/UpdateMemberAndRecords/${this.fullMember?.id}`;
    this.http.put<any>(url, this.newMember).subscribe((response) => {
      console.log(response);
    });
    this.visible = false;
    this.visibleForm = false;
    setTimeout(() => {
      this.fetchData();
    }, 1000);
  }
  createMember() {
    console.log(this.newMember);
    const url = `/api/MinimalApi/CreateMemberAndRecords`;
    this.http.post<any>(url, this.newMember).subscribe((response) => {
      console.log(response);
    });
    this.visible = false;
    this.visibleForm = false;
    setTimeout(() => {
      this.fetchData();
    }, 1000);
  }
  
  deleteMember() {
    const url = `/api/MinimalApi/DeleteMemberAndRecords/${this.fullMember?.id}`;
    this.http.delete<any>(url).subscribe((response) => {
      console.log(response);
    });
    this.visible = false;
    this.visibleForm = false;
    setTimeout(() => {
      this.fetchData();
    }, 1000);
  }
  //#endregion

  //#region picture section
  onUpload(event: UploadEvent) {
    this.messageService.add({
      severity: 'info',
      summary: 'Success',
      detail: 'File Uploaded with Basic Mode',
    });
  }
    //#endregion

  //#region Schemes Section
  visibleSchemes: boolean = false;

  notImmunedCount: number = 0;

  data: any;
  options: any;
  IllsPerDate: number[] = [];
  currentDate = new Date();

  lastMonthName = this.getMonthName(this.currentDate.getMonth() - 1);
  lastMonth =
    this.currentDate.getMonth() === 0 ? 11 : this.currentDate.getMonth(); // Check if current month is January
  daysInLastMonth = new Date(
    this.currentDate.getFullYear(),
    this.lastMonth + 1,
    0
  ).getDate(); // Get number of days in last month
  labels = Array.from({ length: this.daysInLastMonth }, (_, i) => i + 1);
  ngOnInit() {}
  visibleSchemesFunction() {
    if (!this.visibleSchemes) {
      this.getNotImmuned();
      this.GetIllsPerDay();
    }
    this.visibleSchemes = !this.visibleSchemes;
  }
  getNotImmuned() {
    const url = `/api/MinimalApi/getNotImmunedCount`;
    this.http.get<number>(url).subscribe((response) => {
      console.log(response);
      this.notImmunedCount = response;
    });
  }
  GetIllsPerDay() {
    this.http
      .get<any>(`/api/MinimalApi/GetIllsPerDay?month=${new Date().getMonth()}`)
      .subscribe((response) => {
        this.IllsPerDate = response;
        console.log(this.IllsPerDate);
       
       this.data = {
  labels: this.labels,
  datasets: [
    {
      label: `Ills per day in last month: ${this.lastMonthName}`,
      data: this.IllsPerDate,
    },
  ]
}; 
this.options = {
  scales: {
    x: {}, 
    y: {
      beginAtZero: true, 
      aspectRatio: 0.2,
      ticks: {
        stepSize: 1, 
      },
    },
  },
};
      });
  }
  getMonthName(monthNumber: number) {
    const monthNames = [
      'January',
      'February',
      'March',
      'April',
      'May',
      'June',
      'July',
      'August',
      'September',
      'October',
      'November',
      'December',
    ];
    return monthNames[monthNumber];
  }
    //#endregion

}
