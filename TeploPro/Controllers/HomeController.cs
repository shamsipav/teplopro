using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TeploPro.Models;
using TeploPro.Models.HomeViewModels;

namespace TeploPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _context;
        public HomeController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputDataModel input, bool isSaveInputs, int? inputId)
        {
            input.DateOfResult = DateTime.Now;

            if (isSaveInputs)
            {
                _context.InputData.Add(input);
                await _context.SaveChangesAsync();

            }
            if (inputId != null)
            {
                var existInputData = _context.InputData.FirstOrDefault(x => x.Id == inputId);

                if (existInputData != null)
                {
                    existInputData.NumberOfFurnace = input.NumberOfFurnace;
                    existInputData.UsefulVolumeOfFurnace = input.UsefulVolumeOfFurnace;
                    existInputData.UsefulHeightOfFurnace = input.UsefulHeightOfFurnace;
                    existInputData.DiameterOfColoshnik = input.DiameterOfColoshnik;
                    existInputData.DiameterOfRaspar = input.DiameterOfRaspar;
                    existInputData.DiameterOfHorn = input.DiameterOfHorn;
                    existInputData.HeightOfHorn = input.HeightOfHorn;
                    existInputData.HeightOfTuyeres = input.HeightOfTuyeres;
                    existInputData.HeightOfZaplechiks = input.HeightOfZaplechiks;
                    existInputData.HeightOfRaspar = input.HeightOfRaspar;
                    existInputData.HeightOfShaft = input.HeightOfShaft;
                    existInputData.HeightOfColoshnik = input.HeightOfColoshnik;
                    existInputData.EstablishedLevelOfEmbankment = input.EstablishedLevelOfEmbankment;
                    existInputData.NumberOfTuyeres = input.NumberOfTuyeres;
                    existInputData.DailyСapacityOfFurnace = input.DailyСapacityOfFurnace;
                    existInputData.SpecificConsumptionOfCoke = input.SpecificConsumptionOfCoke;
                    existInputData.SpecificConsumptionOfZRM = input.SpecificConsumptionOfZRM;
                    existInputData.ShareOfPelletsInCharge = input.ShareOfPelletsInCharge;
                    existInputData.BlastConsumption = input.BlastConsumption;
                    existInputData.BlastTemperature = input.BlastTemperature;
                    existInputData.BlastPressure = input.BlastPressure;
                    existInputData.BlastHumidity = input.BlastHumidity;
                    existInputData.OxygenContentInBlast = input.OxygenContentInBlast;
                    existInputData.NaturalGasConsumption = input.NaturalGasConsumption;
                    existInputData.ColoshGasTemperature = input.ColoshGasTemperature;
                    existInputData.ColoshGasPressure = input.ColoshGasPressure;
                    existInputData.ColoshGas_CO = input.ColoshGas_CO;
                    existInputData.ColoshGas_CO2 = input.ColoshGas_CO2;
                    existInputData.ColoshGas_H2 = input.ColoshGas_H2;
                    existInputData.Chugun_SI = input.Chugun_SI;
                    existInputData.Chugun_MN = input.Chugun_MN;
                    existInputData.Chugun_P = input.Chugun_P;
                    existInputData.Chugun_S = input.Chugun_S;
                    existInputData.Chugun_C = input.Chugun_C;
                    existInputData.AshContentInCoke = input.AshContentInCoke;
                    existInputData.VolatileContentInCoke = input.VolatileContentInCoke;
                    existInputData.SulfurContentInCoke = input.SulfurContentInCoke;
                    existInputData.SpecificSlagYield = input.SpecificSlagYield;
                    existInputData.HeatCapacityOfAgglomerate = input.HeatCapacityOfAgglomerate;
                    existInputData.HeatCapacityOfPellets = input.HeatCapacityOfPellets;
                    existInputData.HeatCapacityOfCoke = input.HeatCapacityOfCoke;
                    existInputData.AcceptedTemperatureOfBackupZone = input.AcceptedTemperatureOfBackupZone;
                    existInputData.ProportionOfHeatLossesOfLowerPart = input.ProportionOfHeatLossesOfLowerPart;
                    existInputData.AverageSizeOfPieceCharge = input.AverageSizeOfPieceCharge;

                    existInputData.HeatOfBurningOfNaturalGasOnFarms = input.HeatOfBurningOfNaturalGasOnFarms;
                    existInputData.HeatOfIncompleteBurningCarbonOfCoke = input.HeatOfIncompleteBurningCarbonOfCoke;
                    existInputData.TemperatureOfCokeThatCameToTuyeres = input.TemperatureOfCokeThatCameToTuyeres;
                    //existInputData.DateOfResult = DateTime.Now;

                    _context.InputData.Update(existInputData);
                    await _context.SaveChangesAsync();
                }

            }

            return RedirectToAction("Result", input);
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                InputDataModel input = await _context.InputData.FirstOrDefaultAsync(p => p.Id == id);

                ViewData["Variant"] = input.Id;
                ViewData["VariantDateTime"] = input.DateOfResult;

                ViewBag.IsSavedInputData = true;

                if (input != null)
                    return View(input);
            }
            else
            {
                InputDataModel inputData = InputDataModel.GetDefaultData();

                ViewBag.IsSavedInputData = false;

                return View(inputData);
            }
            return NotFound();
        }

        public IActionResult Result(InputDataModel input)
        {
            ResultViewModel viewModel = new ResultViewModel(input);

            return View(viewModel);
        }

        public async Task<IActionResult> Inputs()
        {
            var inputData = await _context.InputData.ToListAsync();

            return View(inputData);
        }

        public IActionResult ComparisonIndex()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Comparison(int? baseInputDataId, int? comparativeInputDataId)
        {
            var basePeriodInput = await _context.InputData.FirstOrDefaultAsync(u => u.Id == baseInputDataId);
            ResultViewModel viewModelBase = new ResultViewModel(basePeriodInput);

            var comparativePeriodInput = await _context.InputData.FirstOrDefaultAsync(u => u.Id == comparativeInputDataId);
            ResultViewModel viewModelComparative = new ResultViewModel(comparativePeriodInput);

            ViewBag.Inputs = await _context.InputData.Where(u => u.isTemperatureCalculate == false).ToListAsync();

            var baseData_1 = viewModelBase.Result.IndexOfTheBottomOfTheFurnace;
            var baseData_2 = viewModelBase.Result.IndexOfTheFurnaceTop;

            var compData_1 = viewModelComparative.Result.IndexOfTheBottomOfTheFurnace;
            var compData_2 = viewModelComparative.Result.IndexOfTheFurnaceTop;


            string baseDataJson_1 = JsonConvert.SerializeObject(baseData_1);
            string baseDataJson_2 = JsonConvert.SerializeObject(baseData_2);

            string compDataJson_1 = JsonConvert.SerializeObject(compData_1);
            string compDataJson_2 = JsonConvert.SerializeObject(compData_2);

            ViewBag.baseData_1 = baseDataJson_1;
            ViewBag.baseData_2 = baseDataJson_2;
            ViewBag.compData_1 = compDataJson_1;
            ViewBag.compData_2 = compDataJson_2;

            return View(new ComparisonViewModel { BasePeriodData = viewModelBase, СomparativePeriodData = viewModelComparative });
        }

        public async Task<IActionResult> Comparison()
        {
            ViewBag.Inputs = await _context.InputData.Where(u => u.isTemperatureCalculate == false).ToListAsync();

            return View();
        }

   
        [HttpPost]
        public async Task<IActionResult> ComparisonTemperature(int? baseInputDataId, int? comparativeInputDataId)
        {
            var baseTemperaturePeriodInput = await _context.InputData.FirstOrDefaultAsync(u => u.Id == baseInputDataId && u.isTemperatureCalculate == true);
            ResultViewModel viewModelBase = new ResultViewModel(baseTemperaturePeriodInput);

            var comparativeTemperaturePeriodInput = await _context.InputData.FirstOrDefaultAsync(u => u.Id == comparativeInputDataId && u.isTemperatureCalculate == true);
            ResultViewModel viewModelComparative = new ResultViewModel(comparativeTemperaturePeriodInput);

            ViewBag.InputsTemperature = await _context.InputData.Where(u => u.isTemperatureCalculate).ToListAsync();

            var baseData = viewModelBase.Result.TheoreticalBurningTemperatureOfCarbonCoke;
            var compData = viewModelComparative.Result.TheoreticalBurningTemperatureOfCarbonCoke;


            string baseDataJson = JsonConvert.SerializeObject(baseData);
            string compDataJson = JsonConvert.SerializeObject(compData);

            ViewBag.baseData = baseDataJson;
            ViewBag.compData = compDataJson;

            return View(new ComparisonViewModel { BasePeriodData = viewModelBase, СomparativePeriodData = viewModelComparative });
        }

        public async Task<IActionResult> ComparisonTemperature()
        {
            ViewBag.InputsTemperature = await _context.InputData.Where(u => u.isTemperatureCalculate).ToListAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
