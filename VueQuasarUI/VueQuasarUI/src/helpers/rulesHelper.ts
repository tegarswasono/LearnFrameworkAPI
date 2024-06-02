//String
const stringRequired = (fieldName: string) => [(val: string) => !!val || fieldName + ' is required']
const emailRequired = (fieldName: string) => [
  (val: string) => !!val || fieldName + ' is required',
  (val: string, rules: any) => rules.email(val) || 'Please enter a valid email address'
]
const passwordRequired = (fieldName: string) => [
  (val: string) => !!val || fieldName + ' is required',
  (val: string) => val.length >= 8 || fieldName + ' must be a minimum of 8 characters',
  (val: string) =>
    (/[A-Z]/.test(val) &&
      /[a-z]/.test(val) &&
      /[0-9]/.test(val) &&
      /[!@#$%^&*(),.?":{}|<>]/.test(val)) ||
    fieldName + ' must contain 1 uppercase letter, 1 lowercase letter, 1 number, and a symbol'
]
const passwordRequiredType1 = (fieldName: string) => [
  (val: string) => !!val || fieldName + ' is required',
  (val: string) => val.length >= 8 || fieldName + ' must be a minimum of 8 characters',
  (val: string) =>
    (/[A-Z]/.test(val) && /[a-z]/.test(val) && /[!@#$%^&*(),.?":{}|<>]/.test(val)) ||
    fieldName + ' must contain 1 uppercase letter, 1 lowercase letter, and a number or symbol'
]
const passwordRequiredType2 = (fieldName: string) => [
  (val: string) => !!val || fieldName + ' is required',
  (val: string) => val.length >= 8 || fieldName + ' must be a minimum of 8 characters',
  (val: string) =>
    (/[A-Z]/.test(val) && /[a-z]/.test(val) && /[!@#$%^&*(),.?":{}|<>]/.test(val)) ||
    fieldName + ' must contain 1 uppercase letter, 1 lowercase letter, and a symbol'
]

//number

//dropdown

//date

//datetime

//textarea

export {
  stringRequired,
  emailRequired,
  passwordRequired,
  passwordRequiredType1,
  passwordRequiredType2
}
