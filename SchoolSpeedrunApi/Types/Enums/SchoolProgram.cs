using System.ComponentModel.DataAnnotations;

namespace SchoolSpeedrunApi.Types.Enums;

public enum SchoolProgram
{
    // Personal & Besökare
    [Display(Name = "Lärare", GroupName = "Personal & Besökare")]
    Teacher = 0,
    
    [Display(Name = "Annan personal", GroupName = "Personal & Besökare")]
    Staff = 1,
    
    [Display(Name = "Besökare / Ej knuten till skolan", GroupName = "Personal & Besökare")]
    Visitor = 2,

    // Högskoleförberedande program
    [Display(Name = "Ekonomiprogrammet", GroupName = "Högskoleförberedande program")]
    Ekonomi = 3,
    
    [Display(Name = "Estetiska programmet – Bild och formgivning", GroupName = "Högskoleförberedande program")]
    EstetBild = 4,
    
    [Display(Name = "Estetiska programmet – Modedesign", GroupName = "Högskoleförberedande program")]
    EstetMode = 5,
    
    [Display(Name = "Estetiska programmet – Musik", GroupName = "Högskoleförberedande program")]
    EstetMusik = 6,
    
    [Display(Name = "Estetiska programmet – Spetsutbildning (MUV)", GroupName = "Högskoleförberedande program")]
    EstetMuv = 7,
    
    [Display(Name = "Naturvetenskapsprogrammet", GroupName = "Högskoleförberedande program")]
    Natur = 8,
    
    [Display(Name = "Samhällsvetenskapsprogrammet", GroupName = "Högskoleförberedande program")]
    Samhall = 9,
    
    [Display(Name = "Teknikprogrammet", GroupName = "Högskoleförberedande program")]
    Teknik = 10,

    // Yrkesprogram
    [Display(Name = "Barn- och fritidsprogrammet", GroupName = "Yrkesprogram")]
    BarnFritid = 11,
    
    [Display(Name = "El- och energiprogrammet", GroupName = "Yrkesprogram")]
    ElEnergi = 12,
    
    [Display(Name = "Fordons- och transportprogrammet", GroupName = "Yrkesprogram")]
    FordonTransport = 13,
    
    [Display(Name = "Naturbruksprogrammet – Hästhållning", GroupName = "Yrkesprogram")]
    Naturbruk = 14,
    
    [Display(Name = "Vård- och omsorgsprogrammet", GroupName = "Yrkesprogram")]
    VardOmsorg = 15,

    // Övrigt
    [Display(Name = "Introduktionsprogrammen", GroupName = "Övrigt")]
    Introduktion = 16,
    
    [Display(Name = "Gymnasial lärlingsutbildning", GroupName = "Övrigt")]
    Larling = 17
}