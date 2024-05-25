export class DataTable{
    success: boolean = true;
    message!: string;
    data!: any[];
    pageNumber: number = 1;
    pageSize: number = 10;
    firstPage!: string;
    lastPage!: string;
    nextPage!: string;
    totalPages: number = 1;
    totalRecords: number = 1;
}