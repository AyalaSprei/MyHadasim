export interface MemberMinimal {
    id:number;
    name: string;
    lastName: string;
  }

  export interface Member {
    id?: number;
    name: string;
    lastName: string;
    identityNumber: string;
    city: string;
    street: string;
    homeNumber: string;
    birthdate?: Date;
    telephone: string;
    mobilePhone: string;
    isImmune: boolean;
    immunes?: Immune[]; // Assuming you also have an Immune interface defined
    illnessId?: number;
    positiveDate?: Date;
    negativeDate?: Date;
    profilePicture?:string
}
export interface Immune {
    immuneId?: number;
    date: Date;
    creatorId: number;
    creatorName?: string;
}
