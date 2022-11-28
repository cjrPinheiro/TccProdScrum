import { Injectable } from '@angular/core';
import * as am5 from '@amcharts/amcharts5';
import * as am5xy from '@amcharts/amcharts5/xy';
import * as am5hierarchy from "@amcharts/amcharts5/hierarchy";
import am5themes_Animated from '@amcharts/amcharts5/themes/Animated';
import { GenericChart } from '@app/models/genericChart';
@Injectable({
  providedIn: 'root'
})


export class ChartService {
public root: am5.Root = {} as am5.Root;

constructor() { }


public loadTreeMapChart(chart: GenericChart){
  this.root = am5.Root.new("treeMapChart");
  console.log(this.root);
  //  if(!this.root.isDisposed())
  //    this.root.dispose();
  const myTheme = am5.Theme.new(this.root);

  myTheme.rule("RoundedRectangle", ["hierarchy", "node", "shape", "depth1"]).setAll({
    strokeWidth: 2
  });

  myTheme.rule("RoundedRectangle", ["hierarchy", "node", "shape", "depth2"]).setAll({
    fillOpacity: 0,
    strokeWidth: 1,
    strokeOpacity: 0.2
  });

  myTheme.rule("Label", ["node", "depth1"]).setAll({
    forceHidden: false
  });

  myTheme.rule("Label", ["node", "depth2"]).setAll({
    fontSize: 10
  });

  this.root.setThemes([
    am5themes_Animated.new(this.root),
    myTheme
  ]);

  // Create wrapper container
  let container = this.root.container.children.push(
    am5.Container.new(this.root, {
      width: am5.percent(100),
      height: am5.percent(100),
      layout: this.root.verticalLayout
    })
  );

  // Create series
  // https://www.amcharts.com/docs/v5/charts/hierarchy/#Adding
  let series = container.children.push(
    am5hierarchy.Treemap.new(this.root, {
      sort: "descending",
      singleBranchOnly: false,
      downDepth: 1,
      upDepth: 0,
      initialDepth: 1,
      valueField: "value",
      categoryField: "name",
      childDataField: "children",
      nodePaddingOuter: 0,
      nodePaddingInner: 0
    })
  );

  series.get("colors")!.set("step", 1);


  container.children.moveValue(
    am5hierarchy.BreadcrumbBar.new(this.root, {
      series: series
    }), 0
  );

  console.log(chart);

  function processData(data:GenericChart) {
    let treeData:any = [];
    data.items.forEach(element => {
      let brandData = { name: element.description, children: [] as any };
      element.childrens.forEach(child=>{
        brandData.children.push({ name: child.description as any, value: child.value });
      });
      treeData.push(brandData);
    });


    return [{
      name: "Visão geral",
      children: treeData
    }];
  }

  series.data.setAll(processData(chart));
  series.set("selectedDataItem", series.dataItems[0]);

}
public loadMemberLineChart(chart: GenericChart){

}
}
