export interface Customer {
    id: number;
    customerId: string;
    firstName: string;
    lastName: string;
    email: string;
    phone: string | null;
    companyName: string | null;
    createdAt: string;
    updatedAt: string | null;
}
